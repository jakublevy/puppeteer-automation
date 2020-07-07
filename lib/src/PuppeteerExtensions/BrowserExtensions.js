import addPageExtensions from "./PageExtensions.js";

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

//Source: https://github.com/puppeteer/puppeteer/issues/443
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

//Returns: [oldActivePage, currActivePage] if page was switched
//         [null, currActivePage] otherwise
async function _activePageInfo(browser, timeoutMs) {
    const oldActivePage = browser.lastActivePage
    const currActivePage = await browser.getActivePage()
    if (currActivePage.target()._targetId !== oldActivePage.target()._targetId) {
        browser.lastActivePage = currActivePage
        return [oldActivePage, currActivePage]
    }
    return [null, currActivePage]

}

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

async function _targetCreatedEventCreated(browser, func) {
    browser.basicOn('targetcreated', async (target) => {
        if(target.type() === 'page') {
            const page = await target.page()
            page.idx = (await browser.pages()).length - 1

            await addPageExtensions(page)

            if('newPageCDPExecute' in browser)
                await page.cdpExecute(browser.newPageCDPExecute)
        }


        await _saveOldPagesDispatch(browser, func, target)
    })
}

async function _targetChangedEventCreated(browser, func) {
    browser.basicOn('targetchanged', async (target) => {
        await _saveOldPagesDispatch(browser, func, target)
    })
}

async function _saveOldPagesDispatch(browser, func, target) {
    await func(target)
    if(target.type() === 'page')
        await (browser.oldPages = await browser.pages())
}

async function _pageClosedEventCreated(browser, func) {
    browser.basicOn('targetdestroyed', async (target) => {
        const pages = await browser.pages()
        if(pages.length < browser.oldPages.length) {
            const closedPage = await browser.findPageById(target._targetId, 'oldPages')
            if(closedPage !== null) {
                func(closedPage)
                browser.oldPages = pages
            }
        }
    })
}
export default addBrowserExtensions
