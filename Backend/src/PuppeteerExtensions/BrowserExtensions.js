/*
Puppeteer browser object custom extensions.
 */

import addPageExtensions from "./PageExtensions.js";

/*
Description: Adds all of extensions to supplied browser object.
 */
async function addBrowserExtensions(browser) {
    browser.getActivePage = (timeoutMs = 500) => _getActivePage(browser, timeoutMs)
    browser.activePageInfo = (timeoutMs = 500) => _activePageInfo(browser, timeoutMs)

    if(browser.basicOn === undefined) {
        browser.basicOn = browser.on
        browser.on = (eventName, func) => _browserCustomOn(browser, eventName, func)
    }
    browser.findPageById = (pageTargetId, where = 'pages') => _findPageById(browser, pageTargetId, where)

    const pages = await browser.pages()
    for(let i = 0; i < pages.length; ++i) {
        pages[i].idx = i
        await addPageExtensions(pages[i])
    }

    browser.lastActivePage = pages[0]
    await browser.getActivePage()
    browser.oldPages = await browser.pages()
}

/*
Description: Gets the currently active page.
             Source: https://github.com/puppeteer/puppeteer/issues/443.
browser: where to look for pages
timeoutMs: the maximum amount of time the function can be executed
 */
async function _getActivePage(browser, timeoutMs) {
    const start = new Date().getTime();
    while(new Date().getTime() - start < timeoutMs) {
        const pages = await browser.pages();
        const arr = [];
        for (const p of pages) {
            if(await p.evaluate(() => { return document.visibilityState === 'visible' })) {
                arr.push(p);
            }
        }
        if(arr.length === 1) {
            browser.lastActivePage = arr[0]
            return arr[0];
        }
    }
    return browser.lastActivePage
}

/*
Returns: [oldActivePage, currActivePage] if page was switched
         [null, currActivePage] otherwise
*/
async function _activePageInfo(browser, timeoutMs) {
    const oldActivePage = browser.lastActivePage
    const currActivePage = await browser.getActivePage()
    if (currActivePage.target()._targetId !== oldActivePage.target()._targetId) {
        browser.lastActivePage = currActivePage
        return [oldActivePage, currActivePage]
    }
    return [null, currActivePage]

}

/*
Description: Finds a page by its _id property.
browser: Which browser to look pages for
pageTargetId: required page _id
where: if containing 'oldPages', we look in browser.oldPages instead of browser.pages() for required page
Returns: matching page if found
         null otherwise
 */
async function _findPageById(browser, pageTargetId, where) {
    let pages = await browser.pages()
    if(where === 'oldPages')
        pages = browser.oldPages

    for(const page of pages) {
        if(page.target()._targetId === pageTargetId)
            return page
    }
    return null
}

/*
Description: This method is called instead of Puppeteer's browser.on.
             We use this to insert our own code that will be called before handlers for some events.
 */
async function _browserCustomOn(browser, eventName, func) {
    if(eventName === 'pageclosed') {
        await _pageClosedEventCreated(browser, func)
    }
    else if(eventName === 'targetcreated') {
        await _targetCreatedEventCreated(browser, func)
    }
    else if(eventName === 'targetchanged') {
        await _targetChangedEventCreated(browser, func)
    }
    else
        browser.basicOn(eventName, func)
}

/*
Description: Called before browser.on('targetcreated', ...).
 */
async function _targetCreatedEventCreated(browser, func) {
    browser.basicOn('targetcreated', async (target) => {
        try {
            if (target.type() === 'page') {
                const page = await target.page()
                page.idx = (await browser.pages()).length - 1

                await addPageExtensions(page)

                if ('newPageCDPExecute' in browser)
                    await page.cdpExecute(browser.newPageCDPExecute)

            }
            await func(target)
        }
        catch(e) {
        }
    })
}

/*
Description: Called before browser.on('targetchanged', ...).
             Prepared like so for future extensions.
 */
async function _targetChangedEventCreated(browser, func) {
    browser.basicOn('targetchanged', async (target) => {
        await func(target)
    })
}

/*
Description: Called before browser.on('targetdestroyed', ...).
             Prepared like so for future extensions.
             We do not implement 'pageClosed' actions, so it doesn't call its second parameter func.
 */
async function _pageClosedEventCreated(browser, func) {
    browser.basicOn('targetdestroyed', async (target) => {

    })
}
export default addBrowserExtensions
