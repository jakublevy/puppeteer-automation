import {readFileSync} from "fs";

let f1, f2, f3

async function addWindowFunctionsToPage(page) {
    if(f1 === undefined)
        f1 = readFileSync('src/selenium-ide/LocatorPkg.js', 'utf-8')
    if(f2 === undefined)
        f2 = readFileSync('src/WindowFunctions/LocatorTransformation.js', 'utf-8')
    if(f3 === undefined)
        f3 = readFileSync('src/WindowFunctions/Xpath.js', 'utf-8')

    await page.addFuncToBrowserWindow(f1, 'LocatorBuilders')
    await page.addFuncToBrowserWindow(f2, 'transformLocators')
    await page.addFuncToBrowserWindow(f3, 'getPath')

}

export default addWindowFunctionsToPage