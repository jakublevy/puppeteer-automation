import zmq from 'zeromq'
import Recorder from "./Recorder.js";
import CodeGenerator from "./CodeGenerator.js";
import Optimizer from "./Optimizer.js";

import {receiveMsg, receiveTimeout, sendAck, sendMsg, die} from './ZeroMQUtils.js'

let sock = new zmq.Pair
sock.connect('tcp://127.0.0.1:3000')

const recorder = new Recorder();
recorder.on('onActionRecorded', (type, action, actions) => sock.send(JSON.stringify(action)))

let browser, codeGen, actions, code, launched
launched = false

async function exitHandler() {
    if(launched)
        await recorder.close()

    sock.close()
}

process.on('exit', exitHandler)
process.on('SIGINT', exitHandler)
process.on('SIGUSR1', exitHandler)
process.on('SIGUSR2', exitHandler)


function processCmd(cmd) {
    return new Promise(async (resolve, reject) => {
        try {
            switch (cmd) {
                case 'setEventsToRecord':
                    cmd = JSON.parse(await receiveMsg(sock))
                    recorder.setEventsToRecord(cmd)
                    break
                case 'launch':
                    launched = true
                    cmd = await receiveMsg(sock) //launchOptions
                    await recorder.launch(JSON.parse(cmd))
                    browser = recorder.getNativeBrowser()
                    break
                case 'connect':
                    cmd = await receiveMsg(sock) //connectOptions
                    await recorder.connect(JSON.parse(cmd))
                    browser = recorder.getNativeBrowser()
                    break
                case 'close':
                    launched = false
                    await recorder.close()
                    break
                case 'disconnect':
                    recorder.disconnect()
                    break
                case 'start':
                    await recorder.start(false)
                    break
                case 'startClean':
                    await recorder.start(true)
                    break
                case 'stop':
                    await recorder.stop()
                    break
                case 'optimize':
                    cmd = JSON.parse(await receiveMsg(sock))
                    const opt = new Optimizer()
                    cmd = opt.optimizeRecordings(cmd)
                    await sendMsg(sock, JSON.stringify(cmd))
                    break
                case 'codeGen':
                    cmd = await receiveMsg(sock) //codeGen options
                    actions = await receiveMsg(sock) //actions
                    codeGen = new CodeGenerator(JSON.parse(cmd))
                    code = codeGen.codeGen(JSON.parse(actions))
                    await sendMsg(sock, code)
                    break
                case 'replay':
                    cmd = await receiveMsg(sock)
                    actions = JSON.parse(await receiveMsg(sock))
                    codeGen = new CodeGenerator(JSON.parse(cmd))
                    codeGen.initForActions(actions)
                    cmd = await receiveMsg(sock)
                    while(cmd !== 'finished')
                    {
                        const code = codeGen.codeGenByIdx(actions, parseInt(cmd))
                        try {
                            await eval(code)
                        }
                        catch(ex) {
                            await sendMsg(ex)
                        }
                        await sendMsg(sock, 'evaluated')
                        cmd = await receiveMsg(sock) //idx
                    }
                    //code = codeGen.codeGen(JSON.parse(actions))
                    //eval(code).then(async o => await sendMsg(sock, 'evaluated'))

                    break
                case 'browserConnectionStatus':
                    await sendMsg(sock, recorder.isBrowserConnected().toString())
                    break

            }
            resolve('OK')
        }
        catch(e) {
            reject(e)
            throw e
        }
    })
}


async function loop() {
    let cmd = await receiveMsg(sock)
    processCmd(cmd)
        .catch(e => sock.send(JSON.stringify({error: e})))
        .then(loop)
}

await loop()