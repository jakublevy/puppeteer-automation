import './JsExtensions/ArrayExtensions.js'
import './JsExtensions/StringExtensions.js'
import './JsExtensions/ObjectExtensions.js'

class CodeGenerator {

    constructor(options = null) {
        this._defaultOptions = {
            addWaitFor: true,
            waitForNavigation: true,
            blankLinesBetweenCodeBlocks: true,
            startPageIdx: 0,
            wrapCodeInAsyncBlock: true,
            awaitAllCalls: true,
            browserClose: false,
            browserDisconnect: true,
            addRequirePuppeteer: false,
            browserConnect: false,
            browserLaunch: true,
            puppeteerOptions: '',
            waitForNavigationOptions: '{ "waitUntil": "networkidle0" }',
            puppeteerLibName: 'puppeteer',
            puppeteerVarName: 'puppeteer',
            browserVarName: 'browser',
            catchErrors : true,
            logErrors : true,
            typeOptions: { delay: 100 },
            indent: 3,
            addXpathFunctions: true,
            clearBeforeChange: true,
            addHoverBeforeClick: false,
            betterLocatorsThanSelector: ['linkText', 'name', 'xpath:innerText']
            /* preferableDataAttribute,

            */
        }
        this._options = options || this._defaultOptions

        this._addMissingOptions()
        this._stringifyOptionObjects()
    }

    _addMissingOptions() {
        for(const [opt, value] of Object.entries(this._defaultOptions)) {
            if(!(opt in this._options))
                this._options[opt] = value
        }

        //headless chrome doesn't understand .hover() (mouseover event)
        //some replacement is required
        this._options.replaceMouseover = !('headless' in this._options.puppeteerOptions) || this._options.puppeteerOptions['headless']
    }

    _stringifyOptionObjects() {
        for(const opt in this._options) {
            if(this._options[opt] instanceof Object)
                this._options[opt] = JSON.stringify(this._options[opt])
        }
    }

    codeGen(actions) {
        this._currentPageIdx = this._options.startPageIdx
        this._alreadyAwaited = []
        this._alreadyAwaited[this._currentPageIdx] = []
        this._permissions = {}
        this._pageIdx2Url = []

        if (!(actions instanceof Array))
            return this._codeGen([actions])

        else
            return this._codeGen(actions)
    }

    _codeGen(actions) {
        let codeBlocks = []
        for (let i = 0; i < actions.length; ++i) {

            this._preprocessAction(actions[i])

            let identifier = this._identifier(actions[i])
            const prologue = this._actPrologue(actions, i, identifier)
            let main

            this._escapeAction(actions[i])

            switch (actions[i].type) {
                case 'pageSwitched':
                    this._currentPageIdx = actions[i].newIdx

                    if (this._alreadyAwaited[this._currentPageIdx] === undefined)
                        this._alreadyAwaited[this._currentPageIdx] = []

                    if (!actions[i].implicitSwitch)
                        main = this._pageFuncCall('bringToFront', [])
                    break

                case 'pageClosed':
                    main = this._pageFuncCall('close', [], actions[i].idx)
                    break

                case 'pageOpened':
                    if(actions[i].explicitlyCreated)
                        main = `const page${actions[i].idx} = ${this._browserFuncCall('newPage', [])}`
                    break

                case 'pageUrlChanged':
                    this._alreadyAwaited[this._currentPageIdx] = []
                    this._pageIdx2Url[this._currentPageIdx] = actions[i].newUrl
                    if (!actions[i].implicitNav) {
                        if (this._options.waitForNavigation && this._options.awaitAllCalls)
                            main = this._pageFuncCall('goto', [actions[i].newUrl], {forceDisableAwait: true})
                        else
                            main = this._pageFuncCall('goto', [actions[i].newUrl])
                    }

                    if(this._options.waitForNavigation && !actions[i].implicitNav)
                        main = this._wrapWithPromiseAll([this._genWaitForNavCall(), main])
                    break

                case 'submit':
                    main = this._genExecRawCodeIdentifier(identifier, 'form', 'form.submit()')
                    break
                case 'click':
                    if(actions[i].emulatedMouseover) {
                        main = this._genIdentifierFuncCall(identifier, 'click', [{delay: 2000}])
                    }
                    else
                        main = this._genIdentifierFuncCall(identifier, 'click', [])
                    break
                case 'dblclick':
                    main = this._genDOMEventFromIdentifier(identifier, 'dblclick')
                    break
                case 'change':
                    if (actions[i].targetType === 'select-one')
                        main = this._genExecRawCodeIdentifier(identifier, 'el', `el.value = \`${actions[i].value}\``)
                    else if(actions[i].targetType === 'radio' || actions[i].targetType === 'checkbox')
                        main = this._genExecRawCodeIdentifier(identifier, 'el', 'el.click()')
                    else // 'text' | 'password' ...
                        main = this._genIdentifierFuncCall(identifier, 'type', [actions[i].value, this._options.typeOptions])
                    break

                case 'scroll':
                    const scrollToFunc = `.scrollTo(${actions[i].coordinates.x}, ${actions[i].coordinates.y})`
                    if (this._scrollingWholePage(actions[i]))
                        main = this._genExecRawCodeInsidePage(`window${scrollToFunc}`)
                    else
                        main = this._genExecRawCodeIdentifier(identifier, 'el', `el.${scrollToFunc}`)
                    break
                case 'select':
                    main = this._genExecRawCodeIdentifier(identifier, 'el', `el.setSelectionRange(${actions[i].selectionStart}, ${actions[i].selectionEnd}, '${actions[i].selectionDirection}')`)
                    break
                case 'copy':
                    this._addPermissionToCurrPage('clipboard-write')
                    const escapedSelection = this._escapeCharacter(actions[i].selection, '`')
                    main = this._genExecRawCodeInsidePage(
                        `const currSelection = window.getSelection().toString()
                                  if(currSelection === '') 
                                     await navigator.clipboard.writeText(\`${escapedSelection}\`)

                                  else 
                                     await navigator.clipboard.writeText(currSelection)`,
                        {asyncCall: true})
                    break
                case 'paste':
                    this._addPermissionToCurrPage('clipboard-read')
                    main = this._genExecRawCodeIdentifier(identifier, 'el', `el.value = await navigator.clipboard.readText()`, true)
                    break
            }

            if (main !== undefined) {
                //next action is an implicit navigation, wrap main of the current one with Promise.all
                if(i + 1 < actions.length && actions[i+1].type === 'pageUrlChanged' && actions[i+1].implicitNav)
                    main = this._wrapWithPromiseAll([this._genWaitForNavCall(), main])



                const epilogue = this._actEpilogue(actions, i)
                codeBlocks.push({prologue, main, epilogue})
            }


        }
        return this._plainCode(codeBlocks)
    }

    _preprocessAction(action) {
        if(action.type === 'mouseover' && this._options.replaceMouseover) {
            action.type = 'click'
            action.emulatedMouseover = true
        }
    }

    _addPermissionToCurrPage(permission) {
        if(this._permissions[this._pageIdx2Url[this._currentPageIdx]] === undefined || !this._permissions[this._pageIdx2Url[this._currentPageIdx]].includes(permission))
            (this._permissions[this._pageIdx2Url[this._currentPageIdx]] = this._permissions[this._pageIdx2Url[this._currentPageIdx]] || []).push(permission)
    }

    _betterLocator(action) {
        if('locators' in action && !action.locators.some(l => l.type === 'id')) {
            const betterLocators = action.locators.filter(e => this._options.betterLocatorsThanSelector.includes(e.type))
            return !betterLocators.empty() ? betterLocators[0].locator : ''
        }
        return ''

    }

    _identifier(action) {
        const locator = this._betterLocator(action)
        if(locator !== '') {
            const [type, value] = locator.split('=')
            return {kind: 'locator', locator: { type: type, value: value, stringValue: locator} }
        }
        return {kind: 'selector', selector: action.selector}
    }

    _genWaitForIdentifier(id) {
        if(id.kind === 'locator')
            return this._pageFuncCall('waitForXPath', [this._genLocatorXpath(id.locator)])

        return this._pageFuncCall('waitForSelector', [id.selector])
    }



    _genLocatorCall(locator) {
        const currentPageVar = `page${this._currentPageIdx}`
        const escapedValue = this._escapeCharacter(locator.value, `'`)
        switch(locator.type) {
            case 'name':
                return `${this._await()}elementByAttribute(${currentPageVar}, 'name', '${escapedValue}')`
            case 'linkText':
                return `${this._await()}elementByLinkText(${currentPageVar}, '${escapedValue}')`
            case 'xpath':
                return `${this._await()}elementByRawXpath(${currentPageVar}, '${escapedValue}')`
        }
    }

    _genLocatorXpath(locator) {
        const escapedValue = this._escapeCharacter(locator.value, `'`)
        switch(locator.type) {
            case 'name':
                return `//*[@name = '${escapedValue}']`
            case 'linkText':
                return `//*[self::a or self::input or self::button][contains(text(), '${escapedValue}')]`
            case 'xpath':
                return locator.value
        }
    }

    _genWaitForNavCall() {
        return this._removeLeadingAwait(this._pageFuncCall('waitForNavigation', [this._options.waitForNavigationOptions]))
    }

    _wrapWithPromiseAll(toWrap) {
        let wrapped = `${this._await()}Promise.all([`

        for(let i = 0; i < toWrap.length; ++i) {
            wrapped = wrapped + `
                         ${toWrap[i]}`

            if(i + 1 < toWrap.length)
                wrapped += ','
        }
        wrapped += '\n                    ])'
        return wrapped
    }

    _removeLeadingAwait(code) {
        if(code instanceof Array)
            return code.map( c => this._removeLeadingAwait(c) )
        else
            return this._options.awaitAllCalls ? code.substring(6) : code
    }

    _codeBlock2Array(cb, removeAwait = false) {
        let p, m, e
        if(removeAwait) {
            p = this._removeLeadingAwait(cb.prologue)
            m = this._removeLeadingAwait(cb.main)
            e = this._removeLeadingAwait(cb.epilogue)
        }
        else {
            p = cb.prologue
            m = cb.main
            e = cb.epilogue
        }

        return [...p, m, ...e]
    }

    _genGivePermissions() {
        let code = ''
        for(const [url, perms] of Object.entries(this._permissions))
            code += `${this._makeIndent()}${this._await()}givePermission(page${this._options.startPageIdx}, context, [${this._makeArgList(perms)}], ${this._makeArgList([url])})\n`

        return code

        // let code = `${this._makeIndent()}const context = ${this._options.browserVarName}.defaultBrowserContext()\n`
        // for(const [url, perms] of Object.entries(this._permissions))
        //     code += `${this._makeIndent()}${this._await()}context.overridePermissions('${this._escapeCharacter(url,`'`)}', [${this._makeArgList(perms)}])\n`
        //
        // return this._permissions.empty() ? '' : code
    }

    _plainCode(codeBlocks) {
        let code = ''
        const c = this._options

        if(this._options.addXpathFunctions) {
            code += 'async function elementByAttribute(page, attrName, attrValue) {\n' +
                '    const xpath = `//*[@${attrName} = \'${attrValue}\']`\n' +
                '    const found = await page.$x(xpath)\n' +
                '    if(found.length > 0)\n' +
                '        return found[0]\n' +
                '    else \n' +
                '        throw new Error(`Element not found: ${xpath}`)\n' +
                '        \n' +
                '}\n' +
                '\n' +
                'async function elementByLinkText(page, linkText) {\n' +
                '    const xpath = `//*[self::a or self::input or self::button][contains(text(), \'${linkText}\')]`\n' +
                '    const found = await page.$x(xpath)\n' +
                '    if(found.length > 0)\n' +
                '        return found[0]\n' +
                '    else\n' +
                '        throw new Error(`Element not found: ${xpath}`)\n' +
                '\n' +
                '}\n' +
                'async function elementByRawXpath(page, xpath) {\n' +
                '    const found = await page.$x(xpath)\n' +
                '    if(found.length > 0)\n' +
                '        return found[0]\n' +
                '    else\n' +
                '        throw new Error(`Element not found: ${xpath}`)\n' +
                '}\n' +
                'async function nativeClickBySelector(page, selector) {\n' +
                '    await page.evaluate(s => document.querySelector(s).click(), selector)\n' +
                '}\n' +
                '\n' +
                'async function nativeClickElementHandle(elementHandle) {\n' +
                '    await elementHandle.evaluate(node => node.click())\n' +
                '}\n' +
                'async function givePermission(page, context, permissions, origin = undefined) {\n' +
                '    const cdpClient = await page.target().createCDPSession()\n' +
                '\n' +
                'async function givePermission(page, context, permissions, origin = undefined) {\n' +
                '    const cdpClient = await page.target().createCDPSession()\n' +
                '\n' +
                '    permissions.includes(\'clipboard-write\') ? permissions.push(\'clipboardSanitizedWrite\') : undefined\n' +
                '    permissions = permissions.map( p => {\n' +
                '        if(p === \'clipboard-read\')\n' +
                '            return \'clipboardReadWrite\'\n' +
                '        else if(p === \'clipboard-write\')\n' +
                '            return \'clipboardReadWrite\'\n' +
                '        return p\n' +
                '    } )\n' +
                '    permissions = [...new Set(permissions)]\n' +
                '\n' +
                '    await cdpClient.send(\'Browser.grantPermissions\', {permissions: permissions, browserContextId: context._id || undefined, origin: origin})\n' +
                '}\n\n'
        }

        if(this._options.addRequirePuppeteer)
            code += `const ${c.puppeteerVarName} = require('${c.puppeteerLibName}');\n\n`

        if(this._options.wrapCodeInAsyncBlock)
            code += '(async () => {\n'

        const startup = c.browserConnect ? 'connect' : (c.browserLaunch ? 'launch' : '')
        if(startup !== '') {
            code += `${this._makeIndent()}const ${c.browserVarName} = ${this._await()}${c.puppeteerVarName}.${startup}(${c.puppeteerOptions})\n`
            code += `${this._makeIndent()}const context = ${this._options.browserVarName}.defaultBrowserContext()\n`
            code += `${this._makeIndent()}const page${c.startPageIdx} = (await ${c.browserVarName}.pages())[${c.startPageIdx}]\n`
            code += this._genGivePermissions() + '\n'
            //code += `${this._makeIndent()}const page0 = ${this._browserFuncCall('newPage', [])}\n\n`
        }

        //process codeBlocks
        for(let i = 0; i < codeBlocks.length; ++i) {
            let joinedBlock = this._codeBlock2Array(codeBlocks[i])
            for(const line of joinedBlock)
                code += `${this._makeIndent()}${line}\n`

            if(c.blankLinesBetweenCodeBlocks && i+1 !== codeBlocks.length)
                code += '\n'
        }


        const shutdown = c.browserClose ? 'close' : (c.browserDisconnect ? 'disconnect' : '')
        if(shutdown !== '')
            code += `\n${this._makeIndent()}${this._browserFuncCall(shutdown, [])}\n`


        if(this._options.wrapCodeInAsyncBlock)
            code += '})()'

        return code
    }

    _actPrologue(actions, i, identifier) {
        let prologue = []

        //preceding action(s) is/are mouseover
        if(i - 1 >= 0 && actions[i - 1].type === 'mouseover' && actions[i].type !== 'mouseover') {
            let j = i - 1
            let hovers = []
            while(j >= 0 && actions[j].type === 'mouseover') {
                const id = this._identifier(actions[j])
                hovers.unshift(this._genIdentifierFuncCall(this._identifier(actions[j]), 'hover', []))
                hovers.unshift(this._genWaitForIdentifier(id))

                --j
            }
            prologue = [...prologue, ...hovers]
        }

        switch(actions[i].type) {
            case 'submit':
            case 'click':
            case 'dblclick':
            case 'change':
            case 'select':
                if(this._options.addWaitFor) {
                    const stringifiedId = JSON.stringify(identifier)
                    if (!this._alreadyAwaited[this._currentPageIdx].includes(stringifiedId)) {
                        prologue.push(this._genWaitForIdentifier(identifier))
                        this._alreadyAwaited[this._currentPageIdx].push(stringifiedId)
                    }
                }
        }

        switch(actions[i].type) {
            case 'click':
            case 'dblclick':
                if(this._options.addHoverBeforeClick && (actions[i].coordinates.x !== 0 || actions[i].coordinates.y !== 0))
                    prologue.push(this._genIdentifierFuncCall(identifier, 'hover', []))
                break
            case 'change':
                if(this._options.clearBeforeChange && !(['radio', 'checkbox', 'select-one'].includes(actions[i].targetType)))
                    prologue.push(this._genExecRawCodeIdentifier(identifier, 'el', `el.value = ''`))
                break
        }
        return prologue
    }

    _actEpilogue(actions, i) {
        let epilogue = []
        switch(actions[i]) {
            //future usages
        }
        return epilogue
    }

    _genDOMEventFromSelector(selector, eventName) {
        const escapedSelector = this._escapeCharacter(selector, '`')
        return this._pageFuncCall('evaluate', [`() => { 
                                let evt = new Event('${eventName}')
                                let el = document.querySelector(\`${escapedSelector}\`)
                                el.dispatchEvent(evt) 
                              }`])
    }

    _genDOMEventFromLocator(locator, eventName) {
        return this._locatorFuncCall(locator, 'evaluate', [`(node) => {
                                let evt = new Event('${eventName}')
                                node.dispatchEvent(evt)
                              }`])
    }

    _genDOMEventFromIdentifier(id, eventName) {
        return id.kind === 'locator' ? this._genDOMEventFromLocator(id.locator, eventName)
                                     : this._genDOMEventFromSelector(id.selector, eventName)
    }


    _genExecRawCodeInsidePage(code, opts = { pageIdx: this._currentPageIdx, asyncCall: false }) {
        opts.pageIdx = opts.pageIdx === undefined ? this._currentPageIdx : opts.pageIdx
        opts.asyncCall = opts.asyncCall === undefined ? false : opts.asyncCall

        let {pageIdx, asyncCall} = opts
        asyncCall = !asyncCall ? '' : 'async '
        return this._pageFuncCall('evaluate', [`${asyncCall}() => {
                                ${code}
                             }`], {pageIdx: pageIdx})
    }

    _genExecRawCodeSelector(selector, nodeName, code, asyncCall = false) {
        asyncCall = asyncCall ? 'async ' : ''

        return this._pageFuncCall('$eval', [selector, `${asyncCall}(${nodeName}) => { 
                                   ${code} 
                               }`])

        // return this._pageFuncCall('evaluate', [`() => {
        //                         const ${nodeName} = document.querySelector(\`${selector}\`)
        //                         ${code}
        //                      }`])
    }

    _genExecRawCodeLocator(locator, nodeName, code, asyncCall = false) {
        asyncCall = asyncCall ? 'async ' : ''
        return this._locatorFuncCall(locator, 'evaluate', [`${asyncCall}(${nodeName}) => {
                                 ${code}
                             }`])
    }

    _genExecRawCodeIdentifier(id, nodeName, code, asyncCall = false) {
        return id.kind === 'locator' ? this._genExecRawCodeLocator(id.locator, nodeName, code, asyncCall)
                                     : this._genExecRawCodeSelector(id.selector, nodeName, code, asyncCall)
    }

    _genIdentifierFuncCall(id, func, args, opts = {pageIdx: this._currentPageIdx}) {
        if(id.kind === 'locator')
            return this._locatorFuncCall(id.locator, func, args)

        else
            return this._pageFuncCall(func, [id.selector, ...args], opts)

    }


    _pageFuncCall(func, args, opts = {pageIdx: this._currentPageIdx, forceDisableAwait: false }) {
        opts.pageIdx = opts.pageIdx === undefined ? this._currentPageIdx : opts.pageIdx
        opts.forceDisableAwait = opts.forceDisableAwait === undefined ? false : opts.forceDisableAwait

        const awaitToAdd = !opts.forceDisableAwait ? this._await() : ''
        let ret
        if(func === 'evaluate' || func === 'evaluateOnNewDocument')
            ret = `${awaitToAdd}page${opts.pageIdx}.${func}(${args})`
        else if(func === '$eval') {
            const [sel, ...rest] = args
            ret = `${awaitToAdd}page${opts.pageIdx}.${func}(${this._makeArgList([sel])}, ${rest})`
        }
        // else if(func === 'click') //selector click call
        //     ret = `${this._await()}nativeClickBySelector(page${pageIdx}, ${this._makeArgList(args)})`
        else
            ret = `${awaitToAdd}page${opts.pageIdx}.${func}(${this._makeArgList(args)})`

        return this._catchAndLogErrors(ret)

    }
    _locatorFuncCall(locator, func, args) {
        let ret
        if(func === 'evaluate' || func === 'evaluateOnNewDocument')
            ret = `${this._await()}(${this._genLocatorCall(locator)}).${func}(${args})`
        else if(func === '$eval') {
            const [sel, ...rest] = args
            ret = `${this._await()}(${this._genLocatorCall(locator)}).${func}(${this._makeArgList([sel])}, ${rest})`
            //ret = `${this._await()}(${this._locatorCall(locator)}).${func}(${args})`
        }
        // else if(func === 'click')
        //     ret = `${this._await()}nativeClickElementHandle(${this._locatorCall(locator)})`
        else
            ret = `${this._await()}(${this._genLocatorCall(locator)}).${func}(${this._makeArgList(args)})`

        return this._catchAndLogErrors(ret)
    }

    _catchAndLogErrors(code) {
        let log = this._options.logErrors ? 'e => console.log(e)' : ''
        let catchEnding = this._options.catchErrors ? `.catch(${log})` : ''
        return code + catchEnding
    }

    _browserFuncCall(func, args) {
        return `${this._await()}${this._options.browserVarName}.${func}(${this._makeArgList(args)})`
    }

    _makeArgList(args) {
        for(let i = 0; i < args.length; ++i) {
            if(args[i] instanceof Object)
                args[i] = JSON.stringify(args[i])

            else if(!args[i].containsJSON() || args[i].containsNumber()) {
                if (args[i][0] !== "'")
                    args[i] = "`" + args[i]


                if (args[i][args[i].length - 1] !== "'")
                    args[i] = args[i] + "`"
            }
        }
        return args.join(', ')
    }

    _scrollingWholePage(action) {
        return action.locators.empty()
    }

    _makeIndent() { return this._options.wrapCodeInAsyncBlock ? ' '.repeat(this._options.indent) : '' }
    _await() { return this._options.awaitAllCalls ? 'await ' : '' }

    _escapeAction(act) {
        act.value = this._escapeCharacter(act.value, '`')
        act.selection = this._escapeCharacter(act.selection, '`')
    }

    _escapeCharacter(str, char) {
        return str !== undefined ? str.split(char).join('\\' + char) : undefined
    }
}

export default CodeGenerator