import {readFileSync} from "fs";

let f1, f2

/*
Description: Adds
                src/WindowFunctions/LocatorTransformation.js,
                src/selenium-ide-src/LocatorPkg.js
             content into browser's window object.
 */
async function addWindowFunctionsToPage(page) {
    if(f1 === undefined)
        f1 = readFileSync('src/selenium-ide-src/LocatorPkg.js', 'utf-8')
    if(f2 === undefined)
        f2 = readFileSync('src/WindowFunctions/LocatorTransformation.js', 'utf-8')

    await page.addFuncToBrowserWindow(f1, 'LocatorBuilders')
    await page.addFuncToBrowserWindow(f2, 'transformLocators')

}

export default addWindowFunctionsToPage