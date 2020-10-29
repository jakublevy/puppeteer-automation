import Optimizer from './Optimizer.js'
import BrowserConnectionLayer from "./BrowserConnectionLayer.js";
import addWindowFunctionsToPage from './WindowFunctionsUtils.js'
import {Mutex} from "async-mutex";


class Recorder extends BrowserConnectionLayer {

    constructor(eventsToRecord = null) {
        super();
        this._internalEventsToRecord = ['keydown', 'keyup', 'mouseover', 'urlHint']
        this._eventsToRecord = eventsToRecord || [
              'click'
            , 'dblclick'
            , 'change' //after changing value of input text or selecting option
            , 'input'
        //    , 'keypress'
            , 'select'
            , 'submit'
            , 'scroll'
            , 'copy'
            , 'paste'

            , 'pageClosed'
            , 'pageSwitched'
            , 'pageOpened'
            , 'pageUrlChanged'
        ]
        this._eventsToRecord = [...this._eventsToRecord, ...this._internalEventsToRecord]

        this._actionRecordedHandler = null

        this.actions = []
        this.optimizedActions = []

        this._stop = false
        this._alreadyStarted = false


        this._mutex = new Mutex()
    }

    async start(cleanStart = true) {
        if(cleanStart)
            this.actions = []

        if(this._stop) {
            this._stop = false
            return
        }

        if(this._alreadyStarted)
            return

        await this.initializeBrowser()
        await this._genStartupHints()

        this._browser.on('targetcreated', async (target) => {
            if(this._stop)
                return

            if(target.type() === 'page') { //new tab was created
                const createdPage = await target.page()
                await this._genPageOpened(createdPage)

                if(!createdPage.url().includes('about:blank') && !createdPage.url().includes('chrome-search://local-ntp/local-ntp.html') && !createdPage.url().includes('chrome://new-tab-page'))
                    await this._urlHint(createdPage.url())



                await this._checkForPageSwitch()
                await this._connectViewportEvents(createdPage)

            }
        })

        this._browser.on('targetchanged', async (target) => {
            if(this._stop)
                return

            if(target.type() === 'page') {
                const changedPage = await target.page()
                if(this._browser.lastActivePage !== undefined && this._browser.lastActivePage.target()._targetId !== target._targetId) {
                    //assumming that page URL cannot be changed while not being the active page
                    await this._genPageSwitched(this._browser.lastActivePage, changedPage)
                    this._browser.lastActivePage = (await this._browser.pages())[changedPage.idx]

                }
                await this._genPageUrlChanged(await target.page(), target.url())
            }

        })

        this._browser.on('pageclosed', async (closedPage) => {
            if(this._stop)
                return

            await this._genPageClosed(closedPage)
            await this._checkForPageSwitch()
        })

        //await this._connectViewportEvents(this._browser.lastActivePage)
        await Promise.all((await this._browser.pages()).map(page => this._connectViewportEvents(page)))
    }

    async stop() {
        this._stop = true
    }

    optimize() {
        const optimizer = new Optimizer()
        this.optimizedActions = optimizer.optimizeActions(JSON.parse(JSON.stringify(this.actions)))
    }

    on(event, handler) {
        //handler will receive:
            // type === 'puppeteer' or type === 'viewport'
            // action
            // alreadyRecordedActions (excluding action), will be automatically added after handler routine is finished
      if(event === 'onActionRecorded')
          this._actionRecordedHandler = handler

    }

    setEventsToRecord(events) {
        this._eventsToRecord = [...events, ...this._internalEventsToRecord]
    }

    removeAllListeners() {
        this._actionRecordedHandler = null
    }

    async _connectViewportEvents(page) {

        const viewportEvents = (eventsToRecord) => {
            const listener = async event => {
                if(event.type === 'keydown' || event.type === 'keyup' || event.type === 'mouseover') {
                    window.__pressedKeys = window.__pressedKeys === undefined ? [] : window.__pressedKeys

                    if(event.type === 'keydown' && !window.__pressedKeys.includes(event.key))
                        window.__pressedKeys.push(event.key.toLowerCase())

                    else if(event.type === 'keyup' && window.__pressedKeys.includes(event.key))
                        window.__pressedKeys = window.__pressedKeys.filter(key => key !== event.key.toLowerCase())

                    if(event.type !== 'mouseover' || !window.__pressedKeys.includes('h'))
                        return
                }


                if(event.target.__handled) {
                    delete event.target.__handled
                    return
                }

                if(event.type === 'submit' || event.type === 'click') {
                    event.preventDefault()
                    event.stopImmediatePropagation()
                }

                let locators, selector
                debugger
                try {
                    const locatorBuilder = new LocatorBuilders(this)
                    const [l, s] = transformLocators(locatorBuilder.buildAll(event.target))
                    locators = l
                    selector = s
                }
                catch (e) {
                    await window.recordingJavascriptMissing()

                    const locatorBuilder = new LocatorBuilders(this)
                    const [l, s] = transformLocators(locatorBuilder.buildAll(event.target))
                    locators = l
                    selector = s
                }

                let eventInfo = {
                    type: event.type
                    , locators
                    , selector
                    , isTrusted: event.isTrusted

                    //in order to know whether an arbitrary event was fired from keyboard by Javascript
                    , keyCode: event.keyCode ? event.keyCode : null
                }

                if(event.type === 'change') {
                    if(event.target.type === 'checkbox' || event.target.type === 'radiobutton')
                        eventInfo.checked = event.target.value

                    eventInfo.value = event.target.value
                    eventInfo.targetType = event.target.type
                }

                else if(event.type === 'input') {
                    eventInfo.data = event.data
                    eventInfo.inputType = event.inputType
                    eventInfo.value = event.target.value
                    eventInfo.targetType = event.target.type
                }

                else if(event.type === 'click' || event.type === 'dblclick') {
                    eventInfo.coordinates = {x: event.clientX, y: event.clientY}
                }
                else if(event.type === 'select') {
                    eventInfo.selection = window.getSelection().toString()
                    eventInfo.selectionStart = event.target.selectionStart
                    eventInfo.selectionEnd = event.target.selectionEnd
                    eventInfo.selectionDirection = event.target.selectionDirection
                }

                else if(event.type === 'copy')
                    eventInfo.selection = window.getSelection().toString()


                else if(event.type === 'scroll')
                    eventInfo.coordinates = {x: this.scrollX, y: this.scrollY}

                await window.captureViewportEvent(eventInfo)

                if(event.type === 'submit' || event.type === 'click') {
                    //handle cases when submit btn has name='submit'
                    event.target.__handled = true
                    const fnc = Object.getPrototypeOf(event.target)[event.type]
                    fnc.call(event.target)
                }
            }
                                                                                              //invoke only once
           eventsToRecord.forEach(eventName => window.addEventListener(eventName, listener, true))
        }

        await addWindowFunctionsToPage(page)
        await page.evaluate(viewportEvents, this._eventsToRecord)
        await page.evaluateOnNewDocument(viewportEvents, this._eventsToRecord)
        await page.exposeFunction('captureViewportEvent', msg => this._captureViewportEvent(msg))
        await page.exposeFunction('recordingJavascriptMissing', () => this._recordingJavascriptMissing())
    }

    async _recordingJavascriptMissing() {
        const pages = await this._browser.pages()
        for(let i = 0; i < pages.length; ++i) {
            await addWindowFunctionsToPage(pages[i])
        }
    }

    async _genPageUrlChanged(page, newUrl) {
        await this._capturePuppeteerEvent({
              type: 'pageUrlChanged'
            , idx : page.idx
            , oldUrl: page.url()
            , newUrl: newUrl
        })
    }

    async _genPageOpened(createdPage) {
        await this._capturePuppeteerEvent({
              type: 'pageOpened'
            , idx: createdPage.idx
            , explicitlyCreated: createdPage.explicitlyCreated
        })
    }

    async _urlHint(url) {
        await this._capturePuppeteerEvent({
            type: 'urlHint'
           , url: url
        })
    }

    async _genPageClosed(closedPage) {
        await this._capturePuppeteerEvent({
              type: 'pageClosed'
            , idx: closedPage.idx
        })
    }

    async _checkForPageSwitch() {
        const [oldPage, currPage] = await this._browser.activePageInfo()
        if (oldPage !== null) { //activePage has switched
            await this._genPageSwitched(oldPage, currPage)
        }
    }

    async _genPageSwitched(oldPage, currPage) {
        await this._capturePuppeteerEvent({
            type: 'pageSwitched'
            , oldIdx: oldPage.idx
            , newIdx: currPage.idx
        })
    }

    async _genStartupHints() {

        await this._capturePuppeteerEvent({
            type: 'startupHints'
          , startupPageIdx: this._browser.lastActivePage.idx
          , existingPagesMaxIdx : (await this._browser.pages()).length - 1

        })
    }

    async _captureViewportEvent(event) {
        if (this._stop)
            return

        await this._checkForPageSwitch()

        if (this._actionRecordedHandler)
            await this._actionRecordedHandler('viewport', event, this.actions)

        this.actions.push(event)
    }

    async _capturePuppeteerEvent(event) {
        if (this._eventsToRecord.includes(event.type) && !this._stop) {
            if (this._actionRecordedHandler)
                await this._actionRecordedHandler('puppeteer', event, this.actions)

            this.actions.push(event)
        }

    }
}

export default Recorder