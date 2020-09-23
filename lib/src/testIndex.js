import Recorder from "./Recorder.js";
import CodeGenerator from "./CodeGenerator.js";
import readline from 'readline'
import clipboardy from 'clipboardy'

const laptopViewport = { width: 1366, height: 768 , deviceScaleFactor: 1, isMobile: false, hasTouch: false, isLandscape: false }

const recorder = new Recorder();
(async () => {
    await recorder.launch({
          headless: false
        //, executablePath: 'C:\\Program Files (x86)\\Chromium\\Application\\chromium.exe'
        , defaultViewport: null
        , devtools: false
    })

  //  await recorder.connect({ browserURL: 'http://localhost:9223', defaultViewport: laptopViewport })

    recorder.setNewPageCDPExecute(cdpCmds)
    await recorder.cdpExecuteAllPages(cdpCmds)

    recorder.on('onActionRecorded', (type, action, actions) => console.log(action))
    //await recorder.start() //TODO: remove
})()

const rl = readline.createInterface({input: process.stdin, output: process.stdout})

rl.on('line', async (line) => {
    if(line === 's')
        await recorder.start()
    else if(line === 'e')
        await recorder.stop()
    else if(line === 'o')
        recorder.optimize()
    else if(line === 'p')
        console.log(recorder.actions)
    else if(line === 'po')
        console.log(recorder.optimizedActions)
    else if(line === 'g') {
        const cg = new CodeGenerator({
           // browserConnect: true,
           // puppeteerOptions: { browserURL: 'http://localhost:9222' }
            puppeteerOptions: { headless: false, slowMo: 100, defaultViewport: null},
            addRequirePuppeteer: false,
            browserDisconnect: false
        })
        const code = cg.codeGen(recorder.optimizedActions)
        clipboardy.writeSync(code)
        console.log(code)

    }
    else if(line === 'gg') {
        const cg = new CodeGenerator({
            puppeteerOptions: { slowMo: 100, defaultViewport: laptopViewport, browserURL: 'http://localhost:9223' },
            addRequirePuppeteer: false,
            browserConnect: true,
            browserLaunch: false,
            browserDisconnect: true,
        })
        const code = cg.codeGen(recorder.optimizedActions)
        clipboardy.writeSync(code)
        console.log(code)
    }
    else if(line === 'r') {
        const cg = new CodeGenerator({
            puppeteerOptions: { headless: false, slowMo: 100, defaultViewport: null},
            addRequirePuppeteer: false,
            browserDisconnect: false,
        })
        const toEval = cg.codeGen(recorder.optimizedActions)
        eval(toEval)
    }
    else if(line === 'rr') {
        const cg = new CodeGenerator({
            puppeteerOptions: { slowMo: 100, defaultViewport: laptopViewport, browserURL: 'http://localhost:9223' },
            addRequirePuppeteer: false,
            browserConnect: true,
            browserLaunch: false,
            browserDisconnect: true,
        })
        const toEval = cg.codeGen(recorder.optimizedActions)
        eval(toEval)
    }

})


const cdpCmds = [
    {
        fName: 'Emulation.setDeviceMetricsOverride',
        args: {
            width: 1366,
            height: 768,
            deviceScaleFactor: 1,
            mobile: false,
            //screenOrientation: { type: 'landscapePrimary', angle: 90 }
        }
    },
    {
        fName: 'Emulation.setTouchEmulationEnabled',
        args: { enabled: false }
    },
    {
        fName: 'Emulation.setEmitTouchEventsForMouse',
        args: {
            enabled: false,
            configuration: 'desktop'
        }
    },
    {
        fName: 'Page.enable', args: {}
    },
    {
        fName: 'Page.setWebLifecycleState', args: {state: 'active'}
    }
]