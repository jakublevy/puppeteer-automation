import Optimizer from './Optimizer.js'
import BrowserConnectionLayer from "./BrowserConnectionLayer.js";
import addWindowFunctionsToPage from './WindowFunctionsUtils.js'


class Recorder extends BrowserConnectionLayer {

    constructor(eventsToRecord = null) {
        super();
        this._internalEventsToRecord = ['keydown', 'keyup', 'mouseover']
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
    }

    async start(cleanStart = true) {
        if(cleanStart)
            this.actions = []

        await this.initializeBrowser()
        await this._genStartupHints()

        this._browser.on('targetcreated', async (target) => {
            if(target.type() === 'page') { //new tab was created
                const createdPage = await target.page()
                await this._genPageOpened(createdPage)
                await this._checkForPageSwitch()
                await this._connectViewportEvents(createdPage)

            }
        })

        this._browser.on('targetchanged', async (target) => {
            if(target.type() === 'page') {
                const changedPage = await target.page()
                if(this._browser.lastActivePage.target()._targetId !== target._targetId) {
                    //assumming that page URL cannot be changed while not being the active page
                    await this._genPageSwitched(this._browser.lastActivePage, changedPage)
                    this._browser.lastActivePage = (await this._browser.pages())[changedPage.idx]

                }
                await this._genPageUrlChanged(await target.page(), target.url())
            }

        })

        this._browser.on('pageclosed', async (closedPage) => {
            await this._genPageClosed(closedPage)
            await this._checkForPageSwitch()
        })

        //await this._connectViewportEvents(this._browser.lastActivePage)
        await Promise.all((await this._browser.pages()).map(page => this._connectViewportEvents(page)))
    }

    async stop() {
        const pages = await this._browser.pages()
        pages.forEach(page => {
            page.removeAllListeners()
            page.clearExposedFunctions()
        })
        this._browser.removeAllListeners()
    }

    optimize() {
        const optimizer = new Optimizer()
        this.optimizedActions = optimizer.optimize(JSON.parse(JSON.stringify(this.actions)))
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
        this._eventsToRecord = events
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

                const locatorBuilder = new LocatorBuilders(this)
                const [locators, selector] = transformLocators(locatorBuilder.buildAll(event.target))

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
                    debugger
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
        await this._checkForPageSwitch()

        if(this._actionRecordedHandler)
            await this._actionRecordedHandler('viewport', event, this.actions)

        this.actions.push(event)
    }

    async _capturePuppeteerEvent(event) {
        if(this._eventsToRecord.includes(event.type)) {
            if (this._actionRecordedHandler)
                await this._actionRecordedHandler('puppeteer', event, this.actions)

            this.actions.push(event)
        }
    }
}

export default Recorder