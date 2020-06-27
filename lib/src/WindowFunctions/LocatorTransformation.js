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

//locators arg example: https://i.imgur.com/Jty5LRv.png