export function parse_locator(locator) {
    if (!locator) {
        throw new TypeError('Locator cannot be empty')
    }
    const result = locator.match(/^([A-Za-z]+)=.+/)
    if (result) {
        let type = result[1]
        const length = type.length
        const actualLocator = locator.substring(length + 1)
        return { type: type, string: actualLocator }
    }
    throw new Error(
        'Implicit locators are obsolete, please prepend the strategy (e.g. id=element).'
    )
}