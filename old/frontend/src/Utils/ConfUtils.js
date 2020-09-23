function getDefaultConf() {
    let defaultConf = JSON.parse(localStorage.getItem('default-conf'))
    if(defaultConf === null) {
        defaultConf = {
            startup: 'launch',
            launchOptions: { path: '"C:\\Program Files (x86)\\Chromium\\Application\\chrome.exe"' },
            connectOptions: {
                endPoint: 'localhost',
                port: '9222',
                sshForward: true,
                user: 'developer',
                pass: 'toor',
                puppeteerOptions: {/* TODO: */}
            }

        }
        //saveDefaultConf(defaultConf)
    }
    return defaultConf
}

function saveDefaultConf(conf) {
    localStorage.setItem('default-conf', JSON.stringify(conf))
}

export {getDefaultConf, saveDefaultConf}