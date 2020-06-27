import {readFileSync} from "fs";

async function addWindowFunctionsToPage(page) {
    await page.addFuncToBrowserWindow(readFileSync('src/selenium-ide/LocatorPkg.js', 'utf-8'), 'LocatorBuilders')
    await page.addFuncToBrowserWindow(readFileSync('src/WindowFunctions/LocatorTransformation.js', 'utf-8'), 'transformLocators')
    await page.addFuncToBrowserWindow(readFileSync('src/WindowFunctions/Xpath.js', 'utf-8'), 'getPath')

}

export default addWindowFunctionsToPage