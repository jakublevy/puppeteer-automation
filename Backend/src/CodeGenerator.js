/*
This file contains all the necessary logic for generating Puppeteer code from captured actions.
 */

import './JsExtensions/ArrayExtensions.js'
import './JsExtensions/StringExtensions.js'
import './JsExtensions/ObjectExtensions.js'
import escape from 'css.escape'

class CodeGenerator {

    constructor(options = null) {
        this._defaultOptions = {

            //Adds waitForSelector or waitForXPath before interacting with an element.
            addWaitFor: true,

            //Defines whether the script should wait for navigation.
            waitForNavigation: true,

            //Adds a blank line between each interaction with an element.
            blankLinesBetweenCodeBlocks: true,

            //idx of an active page when recording started
            startPageIdx: 0,

            //Wraps all actions with (async () => {})()
            wrapCodeInAsyncBlock: true,

            //Puts await before each call.
            awaitAllCalls: true,

            //Adds browser.close() closing statement at the end of the script.
            browserClose: false,

            //Adds browser.disconnect() closing statement at the end of the script.
            browserDisconnect: true,

            //Adds Puppeteer import statement e.g. const puppeteer = require('puppeteer')
            addRequirePuppeteer: false,

            //Defines the startup of a browser to be browser.connect(...).
            //Generated script will connect to the existing browser instance
            browserConnect: false,

            //Defines the startup of a browser to be browser.launch(...).
            //Generated script will start a new browser instance.
            browserLaunch: true,

            //Contains JSON in a string with Puppeteer options for browser.connect/launch
            puppeteerOptions: '',

            //JSON in a string that defines a condition until the script should wait after performing navigation.
            waitForNavigationOptions: '{ "waitUntil": "networkidle0" }',

            //JSON in a string containing a time value that should be waited for elements in waitForSelector
            //and waitForXPath statements.
            waitForTargetOptions: '{ "timeout": 10000 }',

            //Name of Puppeteer package
            puppeteerLibName: 'puppeteer',

            //Name of a variable where Puppeteer is imported to e.g. const puppeteer = ...
            puppeteerVarName: 'puppeteer',

            //Name of a browser variable, e.g. const browser = puppeteer.launch(...)
            browserVarName: 'browser',

            //Defines whether the errors should be caught
            catchErrors : true,

            //Defines whether the errors should be logged.
            logErrors : true,

            //Defines whether the errors should be send using ZeroMQ.js back to Frontend
            //or whether the errors should be printed using console.log
            sendErrorsBack: false,

            //JSON containing a pause (ms) between each keystroke.
            typeOptions: { delay: 100 },

            //Sets the number of spaces for indentation.
            indent: 3,

            //Adds Locators --> XPath conversion and element retrieving methods to the preamble.
            addXpathFunctions: true,

            //Defines whether the content of a field should be cleared before performing recorded action.
            clearBeforeChange: true,

            //Defines whether a mouse should go over an element before performing a click action.
            addHoverBeforeClick: false,

            //Array containing locator types that should be preferred over selectors.
            betterLocatorsThanSelector: ['linkText', 'name', 'xpath:innerText'],

            //Defines whether the line containing puppeteer.launch or puppeteer.connect should be generated.
            addLaunchOrConnect: false,

            //Defines whether the last line of generated code should send a message for Frontend notifying
            //about finished evaluation.
            evaluationFinishedAck: false,

            //Defines whether an implicit page switch caused by opening or closing the page should be ignored.
            ignorePageSwitchAfterPageOpenClose: true
        }
        this._options = options || this._defaultOptions

        this._addMissingOptions()
        this._stringifyOptionObjects()
    }

    /*
    Description: Adds missing options with default values to the new instance of CodeGenerator.
     */
    _addMissingOptions() {
        for(const [opt, value] of Object.entries(this._defaultOptions)) {
            if(!(opt in this._options))
                this._options[opt] = value
        }
    }

    /*
    Description: Stringifies all Object values of this._options
     */
    _stringifyOptionObjects() {
        for(const opt in this._options) {
            if(this._options[opt] instanceof Object)
                this._options[opt] = JSON.stringify(this._options[opt])
        }
    }

    /*
    Description: Performs initialization before actual code generation.
                 The initialization consists of creating new collections, setting variables, etc.
     */
    initForActions(actions) {
        this._alreadyAwaited = []
        this._permissions = {}
        this._pageIdx2Url = []
        this._idxOfDeclaredPages = new Set()
        this._idxOfNewPageToAddToDeclared = -1
        this._processStartupHits(actions)
        this._currentPageIdx = this._options.startPageIdx
    }

    /*
    Description: Generates code from given actions array or single action.
     */
    codeGen(actions) {
        this.initForActions(actions)

        if (!(actions instanceof Array))
            return this._codeGen([actions])

        else
            return this._codeGen(actions)
    }

    /*
    Description: Generates code for single action - actions[idx].
     */
    codeGenByIdx(actions, idx) {
        const [codeBlock, nextIdx] = this._codeGenBlock(actions, idx)
        return this._plainCode([codeBlock])
    }

    /*
    Description: Processes startup hints in order to generate code with respect to them.
                 startup hints contain information about startup page idx and the number of existing pages at startup.
     */
    _processStartupHits(actions) {
        if (!actions.empty() && actions[0].type === 'startupHints') {
            this._options.startPageIdx = actions[0].startupPageIdx
            this._options.existingPagesMaxIdx = actions[0].existingPagesMaxIdx

            for(let i = 0; i <= actions[0].existingPagesMaxIdx; ++i) {
               this._idxOfDeclaredPages.add(i)
                this._alreadyAwaited[i] = []
            }

            actions.shift()
        }
        else {
            this._alreadyAwaited[this._options.startPageIdx] = []
        }

    }

    /*
    Description: Generates code from actions.
    actions: Array of actions
     */
    _codeGen(actions) {
        let codeBlocks = []
        for (let i = 0; i < actions.length; ++i) {
            const [gen, nextI] = this._codeGenBlock(actions, i)
            codeBlocks.push(gen)
            i = nextI
        }
        return this._plainCode(codeBlocks)
    }

    /*
    Description: Generates a block of code from actions[i] consisting of prologue, main, and epilogue.
     */
    _codeGenBlock(actions, i) {
        let identifier = this._identifier(actions[i])
        if(identifier.kind === 'selector' && identifier.selector === undefined)
            identifier.selector = ''

        else if(identifier.kind === 'locator' && (!['name', 'linkText', 'xpath'].includes(identifier.locator.type) || identifier.locator.value === "")) {
            identifier = {kind: 'locator', locator: {type: 'xpath', value: '//*[text() = "Invalid Locator"]', stringValue: 'xpath=//*[text() = "Invalid Locator"]'}}
        }

        let prologue = this._actPrologue(actions, i, identifier)
        let main

        this._escapeAction(actions[i])

        switch (actions[i].type) {
            case 'pageSwitched':
                this._currentPageIdx = actions[i].newIdx

                if (this._alreadyAwaited[this._currentPageIdx] === undefined)
                    this._alreadyAwaited[this._currentPageIdx] = []


                if(this._options.ignorePageSwitchAfterPageOpenClose && i - 1 >= 0 && (actions[i-1].type !== 'pageOpened' && actions[i-1].type !== 'pageClosed')) {
                    if (!actions[i].implicitSwitch)
                        main = this._pageFuncCall('bringToFront', [])
                }
                else if(!this._options.ignorePageSwitchAfterPageOpenClose){
                    if (!actions[i].implicitSwitch)
                        main = this._pageFuncCall('bringToFront', [])
                }
                else if(i === 0 && !actions[i].implicitSwitch) {
                    main = this._pageFuncCall('bringToFront', [])
                }
                break

            case 'pageClosed':
                main = this._pageFuncCall('close', [], {pageIdx: actions[i].idx})
                break

            case 'pageOpened':
                if(actions[i].explicitlyCreated) {
                    const decl = this._declaration(actions[i])
                    main = `${decl}page${actions[i].idx} = ${this._browserFuncCall('newPage', [])}`
                }
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
                    main = this._genIdentifierFuncCall(identifier, 'click', [this._options.waitForTargetOptions])
                break
            case 'dblclick':
                main = this._genDOMEventFromIdentifier(identifier, 'dblclick')
                break
            case 'change':
                if(actions[i].value === undefined)
                    actions[i].value = ''

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
                    main = this._genExecRawCodeIdentifier(identifier, 'el', `el${scrollToFunc}`)
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
            case 'mouseover':
                main = this._genIdentifierFuncCall(identifier, 'hover', [])
                break
        }

        if (main !== undefined) {
            //the next action is an implicit navigation, wrap main of the current one with Promise.all
            if(i + 1 < actions.length && actions[i+1].type === 'pageUrlChanged' && actions[i+1].implicitNav) {
                main = this._wrapWithPromiseAll([this._genWaitForNavCall(), this._removeLeadingAwait(main)])
            }

            const [epilogue, next] = this._actEpilogue(actions, i)
            return [{prologue, main, epilogue}, next - 1]
        }
        return [{prologue: [], main: "", epilogue: []}, i]
    }


    /*
     Description: Adds a permission requirement for the current URL of the active page.
     permission: name of required permission
     */
    _addPermissionToCurrPage(permission) {
        if(this._permissions[this._pageIdx2Url[this._currentPageIdx]] === undefined || !this._permissions[this._pageIdx2Url[this._currentPageIdx]].includes(permission))
            (this._permissions[this._pageIdx2Url[this._currentPageIdx]] = this._permissions[this._pageIdx2Url[this._currentPageIdx]] || []).push(permission)
    }

    /*
      Description: If the supplied action has a better locator(1) than the selector, it is returned.
                   Otherwise '' is returned.

      (1) List of better locators types is this._options.betterLocatorsThanSelector.
     */
    _betterLocator(action) {
        if('locators' in action && !action.locators.some(l => l.type === 'id')) {
            const betterLocators = action.locators.filter(e => this._options.betterLocatorsThanSelector.includes(e.type))
            return !betterLocators.empty() ? betterLocators[0].locator : ''
        }
        return ''

    }

    /*
     Description: Converts given locator in a string to JSON.
     */
    _transformLocator(locator, action) {
        const idx = locator.indexOf('=')
        const type = locator.substring(0, idx)
        const value = locator.substr(idx + 1)
        if(type === 'css')
            return {kind: 'selector', selector: value}
        else if(type === 'id') {
            if(action.selector !== undefined)
                return {kind: 'selector', selector:  '#' + escape(value)}
            else
                return {kind: 'locator', locator: {type: type, value: value, stringValue: locator}}
        }

        else
            return {kind: 'locator', locator: {type: type, value: value, stringValue: locator}}
    }

    /*
     Description: Returns the suitable identifier of an action. It can either be a selector or a locator
     depending on action.target value from Frontend.
     */
    _identifier(action) {
        if('target' in action) {
            if(action.target === 'selector') {
                return {kind: 'selector', selector: action.selector}
            }
            else if(action.target === "-1") {
                return {kind: 'locator', locator: {type: 'xpath', value: '//*[text() = "Missing Locator"]', stringValue: 'xpath=//*[text() = "Missing Locator"]'}}
            }
            else if(action.target.containsNumber()) {
                return this._transformLocator(action.locators[parseInt(action.target)].locator, action)
            }
            else {
                return this._transformLocator(action.target, action)
            }

        }
        else {
            const locator = this._betterLocator(action)
            if (locator !== '') {
                const idx = locator.indexOf('=')
                const type = locator.substring(0, idx)
                const value = locator.substr(idx + 1)
                return {kind: 'locator', locator: {type: type, value: value, stringValue: locator}}
            }
            return {kind: 'selector', selector: action.selector}
        }
    }
    /*
    Description: Generates a waitForXPath or waitForSelector statement based on identifier supplied
     */
    _genWaitForIdentifier(id) {
        if(id.kind === 'locator')
            return this._pageFuncCall('waitForXPath', [this._genLocatorXpath(id.locator), this._options.waitForTargetOptions])

        return this._pageFuncCall('waitForSelector', [id.selector, this._options.waitForTargetOptions])
    }


    /*
    Description: Generates a function call to Locator --> XPath conversion and ElementHandle retrieving method.
     */
    _genLocatorCall(locator) {
        const currentPageVar = `page${this._currentPageIdx}`
        const escapedValue = this._escapeCharacter(locator.value, `'`)
        let call
        switch(locator.type) {
            case 'name':
                call = `${this._await()}elementByAttribute(${currentPageVar}, 'name', '${escapedValue}')`
                break
            case 'linkText':
                call = `${this._await()}elementByLinkText(${currentPageVar}, '${escapedValue}')`
                break
            case 'xpath':
                call = `${this._await()}elementByRawXpath(${currentPageVar}, '${escapedValue}')`
                break
        }
        return this._catchAndLogErrors(call)
    }

    /*
    Description: Generates an XPath statement corresponding to the given locator.
     */
    _genLocatorXpath(locator) {
        const escapedValue = this._escapeCharacter(locator.value, `'`)
        switch(locator.type) {
            case 'name':
                return `//*[@name = '${escapedValue}']`
            case 'linkText':
                return `//*[text() = '${escapedValue}']`
            case 'xpath':
                return locator.value
        }
    }

    /*
    Description: Generates a waitForNavigation function call stripped of leading await
     */
    _genWaitForNavCall() {
        return this._removeLeadingAwait(this._pageFuncCall('waitForNavigation', [this._options.waitForNavigationOptions]))
    }

    /*
    Description: Removes trailing catch of given msg.
     */
    _removeTrailingCatch(msg) {
        const idx = msg.lastIndexOf(".catch")
        if(idx !== -1)
           return msg.substring(0, idx)
        return msg
    }

    /*
    Description: Generates Promise.all statement containing joined lines from toWrap Array.
     */
    _wrapWithPromiseAll(toWrap) {
        let wrapped = `${this._await()}Promise.all([`

        for(let i = 0; i < toWrap.length; ++i) {
            let tW = toWrap[i]
            if(this._options.catchErrors) {
                tW = this._removeTrailingCatch(toWrap[i])
            }
            wrapped = wrapped + `
                         ${tW}`

            if(i + 1 < toWrap.length)
                wrapped += ','
        }
        wrapped += '\n                    ])'
        return this._catchAndLogErrors(wrapped)
    }

    /*
    Description: Removes leading await of code parameter
                 (in case parameter is Array, it removes code from every element of Array)
     */
    _removeLeadingAwait(code) {
        if(code instanceof Array)
            return code.map( c => this._removeLeadingAwait(c) )
        else
            return this._options.awaitAllCalls ? code.substring(6) : code
    }

    /*
    Description: Converts codeblock generated from _codeGenBlock function into Array.
     */
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

    /*
    Description: Generates givePermission calls based on previous _addPermissionToCurrPage calls.
     */
    _genGivePermissions() {
        let code = ''
        for(const [url, perms] of Object.entries(this._permissions))
            code += `${this._makeIndent()}${this._await()}givePermission(page${this._options.startPageIdx}, context, [${this._makeArgList(perms)}], ${this._makeArgList([url])})\n`

        return code
    }

    /*
    Description: Converts code blocks generated by _codeGenBlock into plain code.
     */
    _plainCode(codeBlocks) {
        let code = ''
        const c = this._options

        if(this._options.addXpathFunctions) {
            code += 'async function elementByAttribute(page, attrName, attrValue) {\n' +
                '    const xpath = `//*[@${attrName} = \'${attrValue}\']`\n' +
                '    const found = await page.$x(xpath)\n' +
                '    if(found.length === 1)\n' +
                '        return found[0]\n' +
                '    else if (found.length === 0)\n' +
                '        throw new Error(`Element not found: ${xpath}`)\n' +
                '    else {\n' +
                '        await sendMsg(sock, `Warning Xpath: ${xpath} is ambiguous`)\n' +
                '        return found[0]\n' +
                '    }\n' +
                '\n' +
                '}\n' +
                '\n' +
                'async function elementByLinkText(page, linkText) {\n' +
                '    const xpath = `//*[text() = \'${linkText}\']`\n' +
                '    const found = await page.$x(xpath)\n' +
                '    if(found.length === 1)\n' +
                '        return found[0]\n' +
                '    else if (found.length === 0)\n' +
                '        throw new Error(`Element not found: ${xpath}`)\n' +
                '    else {\n' +
                '        await sendMsg(sock, `Warning Xpath: ${xpath} is ambiguous`)\n' +
                '        return found[0]\n' +
                '    }\n' +
                '\n' +
                '}\n' +
                'async function elementByRawXpath(page, xpath) {\n' +
                '    const found = await page.$x(xpath)\n' +
                '    if(found.length === 1)\n' +
                '        return found[0]\n' +
                '    else if (found.length === 0)\n' +
                '        throw new Error(`Element not found: ${xpath}`)\n' +
                '    else {\n' +
                '        await sendMsg(sock, `Warning Xpath: ${xpath} is ambiguous`)\n' +
                '        return found[0]\n' +
                '    }\n' +
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
            if(this._options.addLaunchOrConnect)
                code += `${this._makeIndent()}const ${c.browserVarName} = ${this._await()}${c.puppeteerVarName}.${startup}(${c.puppeteerOptions})\n`

            if(!this._permissions.empty())
                code += `${this._makeIndent()}const context = ${this._options.browserVarName}.defaultBrowserContext()\n`

            code += `${this._makeIndent()}const pages = await ${this._options.browserVarName}.pages()\n`

            this._idxOfDeclaredPages.forEach((k,value) => code += `${this._makeIndent()}let page${value} = pages[${value}]\n`)
            if(this._idxOfNewPageToAddToDeclared !== -1) {
                this._idxOfDeclaredPages.add(this._idxOfNewPageToAddToDeclared)
                this._idxOfNewPageToAddToDeclared = -1
            }
            code += '\n'
            code += this._genGivePermissions() + '\n'
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

        if(this._options.evaluationFinishedAck)
            code += `${this._makeIndent()}await sendMsg(sock, "evaluated")\n`

        if(this._options.wrapCodeInAsyncBlock)
            code += '})()'

        return code
    }

    /*
    Description: Generates an prologue of actions[i] identified by identifier.
                 E.g. a prologue is cleaning of a field before typing.
     */
    _actPrologue(actions, i, identifier) {
        let prologue = []

        switch(actions[i].type) {
            case 'submit':
            case 'click':
            case 'dblclick':
            case 'change':
            case 'select':
            case 'mouseover':
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

    /*
    Description: Generates an epilogue of actions[i].
                 E.g. an epilogue is the creation and initialization of a page variable after the new page has been opened
     */
    _actEpilogue(actions, i) {
        let epilogue = []
        if(i + 1 < actions.length) {
            if(actions[i+1].type === 'pageOpened') {
                if(!actions[i+1].explicitlyCreated) {
                    const decl = this._declaration(actions[i+1])
                    epilogue.push(`${decl}page${actions[i + 1].idx} = (await ${this._options.browserVarName}.pages())[${actions[i + 1].idx}]`)
                }
            }
        }
        return [epilogue, i+1]
    }

    /*
    Description: Generates a declaration statement if a page wasn't already declared.
     */
    _declaration(act) {
        const decl = this._idxOfDeclaredPages.has(act.idx) ? '' : 'let '
        this._idxOfNewPageToAddToDeclared = act.idx
        return decl
    }

    /*
    Description: Generates page.evaluate call that contains DOM event that will be executed on a page.
    selector: selector of an element that fires the event
    eventName: the name of the event that will be fired
     */
    _genDOMEventFromSelector(selector, eventName) {
        const escapedSelector = this._escapeCharacter(selector, '`')
        return this._pageFuncCall('evaluate', [`() => { 
                                let evt = new Event('${eventName}')
                                let el = document.querySelector(\`${escapedSelector}\`)
                                el.dispatchEvent(evt) 
                              }`])
    }

    /*
    Description: Generates ElementHandle.evaluate call that contains DOM event that will be executed on a page.
    locator: locator of an element that fires the event
    eventName: the name of the event that will be fired
     */
    _genDOMEventFromLocator(locator, eventName) {
        return this._locatorFuncCall(locator, 'evaluate', [`(node) => {
                                let evt = new Event('${eventName}')
                                node.dispatchEvent(evt)
                              }`])
    }

    /*
    Description: Generates page.evaluate ElementHandle.evaluate call that contains DOM event that will be executed on a page.
                 page.evaluate or ElementHandle.evaluate will be selected based on the id parameter.
    id: identifier of an element that fires the event (either selector or locator)
    eventName: the name of the event that will be fired
     */
    _genDOMEventFromIdentifier(id, eventName) {
        return id.kind === 'locator' ? this._genDOMEventFromLocator(id.locator, eventName)
                                     : this._genDOMEventFromSelector(id.selector, eventName)
    }


    /*
    Description: Generates page.evaluate call that contains arbitrary JavaScript that will be executed on a page.
    code: code to execute on a page
    opts: { pageIdx: page identifier, asyncCall: whether the lambda method should be async }
     */
    _genExecRawCodeInsidePage(code, opts = { pageIdx: this._currentPageIdx, asyncCall: false }) {
        opts.pageIdx = opts.pageIdx === undefined ? this._currentPageIdx : opts.pageIdx
        opts.asyncCall = opts.asyncCall === undefined ? false : opts.asyncCall

        let {pageIdx, asyncCall} = opts
        asyncCall = !asyncCall ? '' : 'async '
        return this._pageFuncCall('evaluate', [`${asyncCall}() => {
                                ${code}
                             }`], {pageIdx: pageIdx})
    }

    /*
    Description: Generates page.$eval(selector, ...) call that passes an element obtained from a selector into
                 a function block. This function block will be executed inside a page. The variable corresponding
                 to the element will be named nodeName.
    selector: selector of an element
    nodeName: name of the variable that corresponds to the selector.
              This name will be used in JavaScript that will be executed inside a page.
    asyncCall: whether the lambda method should be async
     */
    _genExecRawCodeSelector(selector, nodeName, code, asyncCall = false) {
        asyncCall = asyncCall ? 'async ' : ''

        return this._pageFuncCall('$eval', [selector, `${asyncCall}(${nodeName}) => { 
                                   ${code} 
                               }`])

    }

    /*
    Description: Generates ElementHandler.evaluate(...) call that passes an element obtained from a locator into
                 a function block. This function block will be executed inside a page. The variable corresponding
                 to the element will be named nodeName.
    locator: locator of an element
    nodeName: name of the variable that corresponds to the selector.
              This name will be used in JavaScript that will be executed inside a page.
    asyncCall: whether the lambda method should be async
     */
    _genExecRawCodeLocator(locator, nodeName, code, asyncCall = false) {
        asyncCall = asyncCall ? 'async ' : ''
        return this._locatorFuncCall(locator, 'evaluate', [`${asyncCall}(${nodeName}) => {
                                 ${code}
                             }`])
    }

    /*
    Description: Generates either _genExecRawCodeSelector or _genExecRawCodeLocator, see their docs for more info.
                 The function is selected based on id, if it is locator, _genExecRawCodeLocator will be selected
                                                       if it is selector, _genExecRawCodeSelector will be selected
     */
    _genExecRawCodeIdentifier(id, nodeName, code, asyncCall = false) {
        return id.kind === 'locator' ? this._genExecRawCodeLocator(id.locator, nodeName, code, asyncCall)
                                     : this._genExecRawCodeSelector(id.selector, nodeName, code, asyncCall)
    }

    /*
     Description: Generates arbitrary function call either for ElementHandle object
                  or Page object that expects a selector as its first argument.
     id: identifier of element
     func: function which call to generate
     args: args that will be supplied to func call
     opts: { pageIdx: page identifier }
     */
    _genIdentifierFuncCall(id, func, args, opts = {pageIdx: this._currentPageIdx}) {
        if(id.kind === 'locator')
            return this._locatorFuncCall(id.locator, func, args)

        else
            return this._pageFuncCall(func, [id.selector, ...args], opts)

    }

    /*
    Description: Generates arbitrary function call for a Page object.
    func: function which call to generate
    args: args that will be supplied to func call
    opts: { pageIdx: page identifier, forceDisableAwait: if true, generated statement will never start with await,
                                                         otherwise the presence of await depends on this._options.awaitAllCalls }
     */
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
        else
            ret = `${awaitToAdd}page${opts.pageIdx}.${func}(${this._makeArgList(args)})`

        return this._catchAndLogErrors(ret)

    }
    /*
    Description: Generates arbitrary function call for ElementHandle object.
    locator: identifier for ElementHandle object
    func: function which call to generate
    args: args that will be supplied to func call
     */
    _locatorFuncCall(locator, func, args) {
        let ret
        if(func === 'evaluate' || func === 'evaluateOnNewDocument')
            ret = `${this._genLocatorCall(locator)}.then(el => el.${func}(${args}))`
        else if(func === '$eval') {
            const [sel, ...rest] = args
            ret = `${this._genLocatorCall(locator)}.then(el => el.${func}(${this._makeArgList([sel])}, ${rest}))`
        }
        else
            ret = `${this._genLocatorCall(locator)}.then(el => el.${func}(${this._makeArgList(args)}))`

        return this._catchAndLogErrors(ret)
    }

    /*
    Description: Generates catch (1) and log (2) code that is stuck to every statement.

    (1) if this._options.catchErrors
    (2) if this._options.logErrors is true
     */
    _catchAndLogErrors(code) {
        let logInner = this._options.sendErrorsBack ? 'async e => await sendMsg(sock, e)' : 'e => console.log(e)'

        let log = this._options.logErrors ? logInner : ''
        let catchEnding = this._options.catchErrors ? `.catch(${log})` : ''
        return code + catchEnding
    }

    /*
    Description: Generates a function call with a browser variable.
    func: name of a function to call
    args: arguments to supply into generated function call
    E.g. browser.newPage() (with no arguments)
     */
    _browserFuncCall(func, args) {
        return `${this._await()}${this._options.browserVarName}.${func}(${this._makeArgList(args)})`
    }

    /*
    Description: Converts given args Array into function arguments separated by a comma.
     */
    _makeArgList(args) {
        for(let i = 0; i < args.length; ++i) {
            if(args[i] instanceof Object)
                args[i] = JSON.stringify(args[i])

            else if(!args[i].containsJSON() || args[i].containsNumber()) {
                if(args[i] === '') {
                    args[i] = "''"
                    continue
                }

                if (args[i][0] !== "'" && args[i][0] !== "`")
                    args[i] = "`" + args[i]


                if (args[i][args[i].length - 1] !== "'" && args[i][args[i].length - 1] !== "`")
                    args[i] = args[i] + "`"
            }
        }
        return args.join(', ')
    }

    /*
    Description: Returns true if the whole page was scrolled.
                         false if a specific element was scrolled.
     */
    _scrollingWholePage(action) {
        return action.locators.empty()
    }

    _makeIndent() { return this._options.wrapCodeInAsyncBlock ? ' '.repeat(this._options.indent) : '' }
    _await() { return this._options.awaitAllCalls ? 'await ' : '' }

    /*
     Description: Escapes the action's properties value and selection
     */
    _escapeAction(act) {
        act.value = this._escapeCharacter(act.value, '`')
        act.selection = this._escapeCharacter(act.selection, '`')
    }

    /*
    Description: Escapes every occurrence of char in str.
     */
    _escapeCharacter(str, char) {
        return str !== undefined ? str.split(char).join('\\' + char) : undefined
    }

}

export default CodeGenerator