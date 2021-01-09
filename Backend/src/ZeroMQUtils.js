/*
Utitilies to make working with ZeroMQ.js even simpler.
 */

async function receiveMsg(sock) {
    return (await sock.receive()).toString('utf-8')
}

async function sendAck(sock) {
    await sock.send('ACK')
}

async function sendNak(sock) {
    await sock.send('NAK')
}

async function sendMsg(sock, msg) {
    await sock.send(msg)
}

export {receiveMsg, sendAck, sendNak, sendMsg}