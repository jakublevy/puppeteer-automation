import zmq from 'zeromq'
import Recorder from "./Recorder.js";
import CodeGenerator from "./CodeGenerator.js";

let sock = new zmq.Pair
sock.connect('tcp://127.0.0.1:3000')

async function receiveMsg() {
    return (await sock.receive()).toString("utf-8")
}

async function sendAck() {
    await sock.send('OK')
}

const recorder = new Recorder();
recorder.on('onActionRecorded', (type, action, actions) => sock.send(JSON.stringify(action)))
while (true) {
    let response = await receiveMsg()
    switch (response) {
        case 'setEventsToRecord':
            response = JSON.parse(await receiveMsg())
            recorder.setEventsToRecord(response)
            //TODO:
            break
        case 'launch':
            response = await receiveMsg() //launchOptions
            await recorder.launch(JSON.parse(response))
            await sendAck()
            break
        case 'connect':
            response = await receiveMsg() //connectOptions
            await recorder.connect(JSON.parse(response))
            await sendAck()
            break
        case 'close':
            await recorder.close()
            await sock.send('OK')
            break
        case 'disconnect':
            recorder.disconnect()
            await sock.send('OK')
            break
        case 'start':
            await recorder.start()
            break
        case 'startClean':
            recorder.clear()
            await recorder.start()
            break
        case 'stop':
            await recorder.stop()
            break
        case 'browserConnectionStatus':
            await sock.send(recorder.isBrowserConnected().toString())
            break

    }
}
