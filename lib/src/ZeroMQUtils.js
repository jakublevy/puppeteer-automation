async function receiveMsg(sock) {
    return (await sock.receive()).toString('utf-8')
}

async function receiveTimeout(sock, timeout) {
    const start = Date.now()
    while(Date.now() - start <= timeout) {
        if(sock.readable) {
            return [true, (await sock.receive()).toString('utf-8')]
        }
    }
    return [false, null]
}

async function sendAck(sock) {
    await sock.send('OK')
}

async function sendMsg(sock, msg) {
    await sock.send(msg)
}

async function die(sock) {
    await sock.send('dying')
    process.exit(0)
}

export {receiveMsg, receiveTimeout, sendAck, sendMsg, die}