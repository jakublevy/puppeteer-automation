import { createContext } from 'react'

const fetchConfig = () => {
    const conf = JSON.parse(localStorage.getItem('configuration'))
    if(conf === null) {
        return {
            address: '',
            port: '',
            user: '',
            pass: ''
        }
    }
    return conf
}

const saveConfig = (conf) => localStorage.setItem('configuration', JSON.stringify(conf))

const configFilled = (conf) => {
    let missingFields = []
    let filled = true
    if(conf.address === '') {
        missingFields.push('address')
        filled = false
    }
    if(conf.port === '') {
        missingFields.push('port')
        filled = false
    }
    if(conf.user === '') {
        missingFields.push('user')
        filled = false
    }
    if(conf.pass === '') {
        missingFields.push('pass')
        filled = false
    }
    return [filled, missingFields]
}

const DataContext = createContext()
const {Provider, Consumer} = DataContext

export { Provider as DataProvider, Consumer as DataConsumer, fetchConfig, saveConfig, configFilled}
export default DataContext