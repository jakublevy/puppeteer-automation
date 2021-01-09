import puppeteer from "puppeteer";
import addBrowserExtensions from "./PuppeteerExtensions/BrowserExtensions.js";

/*
Layer of abstraction containing basic Puppeteer <-> Browser connection API.
This file also contains a method that adds custom extensions to Puppeteer browser object.
 */

class BrowserConnectionLayer {

    constructor(browser = null) {
        this._browser = browser
    }

    isBrowserConnected() {
        return this._browser !== null && this._browser.isConnected()
    }

    async launch(options) {
        this._browser = await puppeteer.launch(options)
        await this.initializeBrowser()
    }

    async connect(options) {
        this._browser = await puppeteer.connect(options)
        await this.initializeBrowser()
    }

    getNativeBrowser() {
        return this._browser
    }

    disconnect() {
        if(this._browser !== null && this._browser.isConnected()) {
            this._browser.disconnect()
            this._browser = null
        }
    }

    async close() {
        if(this._browser !== null && this._browser.isConnected()) {
            await this._browser.close()
            this._browser = null
        }
    }

    /*
    Description: Executes supplied CDP methods when a new page is created. Methods are executed by their give names
                 and supplied parameters.
    cmds: [
    { fName: function name to execute, args: {name: value or object, ...} },
    ...]
    */
    setNewPageCDPExecute(cmds) {
        this._browser.newPageCDPExecute = cmds
    }

    /*
    Description: Executes supplied CDP methods right now on all existing pages. Methods are executed by their give names
                 and supplied parameters.
    cmds: [
    { fName: function name to execute, args: {name: value or object, ...} },
    ...]
    */
    async cdpExecuteAllPages(cmds) {
        for (const page of await this._browser.pages())
            await page.cdpExecute(cmds)
    }

    /*
    Description: This method adds custom Puppeteer browser extensions to a browser object.
     */
    async initializeBrowser() {
        await addBrowserExtensions(this._browser)
    }
}

export default BrowserConnectionLayer