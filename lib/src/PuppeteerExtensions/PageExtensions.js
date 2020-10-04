async function addPageExtensions(page) {
    page.removeExposedFunctionByName = fName => _removeExposedFunctionByName(page, fName)
    page.clearExposedFunctionsBindings = () => _clearExposedFunctionsBindings(page)
    page.cdpExecute = cmds => _cdpExecute(page, cmds)
    page.cdpOneShot = (funcName, ...args) => _cdpOneShot(page, funcName, ...args)
    page.addFuncToBrowserWindow = (funcCode, accessName) => _addFuncToBrowserWindow(page, funcCode, accessName)
    page.explicitlyCreated = page.url() === 'about:blank' || page.url() === 'chrome-search://local-ntp/local-ntp.html'
}

function _removeExposedFunctionByName(page, fName) {
    return page._pageBindings.delete(fName)
}

function _clearExposedFunctionsBindings(page) {
    page._pageBindings.clear()
}

async function _cdpExecute(page, cmds) {
    const cdpClient = await page.target().createCDPSession()
    for(const {fName, args} of cmds)
        await cdpClient.send(fName, args)
}

async function _cdpOneShot(page, funcName, ...args) {
    const cdpClient = await page.target().createCDPSession()
    return (await cdpClient.send(funcName, ...args))
}

async function _addFuncToBrowserWindow(page, funcCode, accessName) {
    await page.evaluate( (code, name) => eval("window." + name + " = " + code), funcCode, accessName)
    await page.evaluateOnNewDocument( (code, name) => eval("window." + name + " = " + code), funcCode, accessName)
}

export default addPageExtensions