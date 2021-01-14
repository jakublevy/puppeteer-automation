/*
Methods extending Puppeteer's page object functionality.
 */

/*
Description: Adds all of extensions to supplied page object.
 */
async function addPageExtensions(page) {
    page.removeExposedFunctionByName = fName => _removeExposedFunctionByName(page, fName)
    page.clearExposedFunctionsBindings = () => _clearExposedFunctionsBindings(page)
    page.cdpExecute = cmds => _cdpExecute(page, cmds)
    page.addFuncToBrowserWindow = (funcCode, accessName) => _addFuncToBrowserWindow(page, funcCode, accessName)
    page.explicitlyCreated = page.url() === 'about:blank' || page.url() === 'chrome-search://local-ntp/local-ntp.html' || page.url() === 'chrome://new-tab-page/'
}

/*
Description: page.exponseFuction has not delete counterpart.
             This method fills the gap.
 */
function _removeExposedFunctionByName(page, fName) {
    return page._pageBindings.delete(fName)

}

/*
Description: Deletes all exposed functions.
 */
function _clearExposedFunctionsBindings(page) {
    page._pageBindings.clear()
}

/*
Description: Executes supplied CDP methods right now on all existing pages. Methods are executed by their give names
             and supplied parameters.
cmds: [
    { fName: function name to execute, args: {name: value or object, ...} },
    ...
]
*/
async function _cdpExecute(page, cmds) {
    const cdpClient = await page.target().createCDPSession()
    for(const {fName, args} of cmds)
        await cdpClient.send(fName, args)
}

/*
Description: Exposes given function code to page's window object.
page: which page should the function be exposed to
funcCode: the code of a function that will be exposed to window object
accessName: new name that will represent the function inside window object,
            the function will be accessible as e.g. window.accessName.
 */
async function _addFuncToBrowserWindow(page, funcCode, accessName) {
    await page.evaluate( (code, name) => eval("window." + name + " = " + code), funcCode, accessName)
    await page.evaluateOnNewDocument( (code, name) => eval("window." + name + " = " + code), funcCode, accessName)
}

export default addPageExtensions