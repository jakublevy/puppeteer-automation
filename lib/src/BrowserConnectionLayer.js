import puppeteer from "puppeteer";
import addBrowserExtensions from "./PuppeteerExtensions/BrowserExtensions.js";

class BrowserConnectionLayer {

    constructor(browser = null) {
        this._browser = browser
    }

    async launch(options) {
        this._browser = await puppeteer.launch(options)
        await this._browserInitialized()
    }

    async connect(options) {
        this._browser = await puppeteer.connect(options)
        await this._browserInitialized()
    }

    disconnect() {
        if(this._browser !== null && this._browser.isConnected()) {
            this._browser.disconnect()
            this._browser = null
        }
    }

    //[{fName: function name to execute, args: {...} }, ...]
    setNewPageCDPExecute(cmds) {
        this._browser.newPageCDPExecute = cmds
    }

    async cdpExecuteAllPages(cmds) {
        // if('newPageCDPExecute' in this._browser) {
        //     for (const page of await this._browser.pages())
        //         await page.cdpExecute(this._browser.newPageCDPExecute)
        // }

        for (const page of await this._browser.pages())
            await page.cdpExecute(cmds)
    }

    async _browserInitialized() {
        await addBrowserExtensions(this._browser)
    }
}

export default BrowserConnectionLayer