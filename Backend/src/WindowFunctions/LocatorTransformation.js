/*
Description: Returns given locators with a selector in a tuple.
             The selector is extracted CSS locator.

   locators arg. example: [ ['css=input.lightBtn', 'css:finder'], ['xpath=//button[contains(text(), 'Login')]', 'xpath::innerText'] ]
 */
function transformLocators(locators) {
    let transformed = []
    let selector
    for(const [locator, type] of locators) {
        if(type === 'css:finder')
            selector = locator.substring(4) //remove leading 'css='

        transformed.push({locator, type})
    }

    return [transformed, selector]
}
