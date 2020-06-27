import React from 'react'
import { DataConsumer, saveConfig } from '../dataContext'

const Settings = () => {
    return (
        <DataConsumer>
            {({ config, setConfig }) => {
                const showInvalid = elm => {
                    if(!elm.classList.contains('is-invalid')) {
                        elm.classList.add('is-invalid')
                    }
                }

                const removeValidationHints = elm => {
                    elm.classList.remove('is-invalid')
                    elm.classList.remove('is-valid')
                }

                const fieldChanged = e => {
                    const elm = e.target
                    if (e.target.checkValidity()) {
                        config[elm.name] = elm.value
                        setConfig(config)
                        saveConfig(config)
                        removeValidationHints(elm)
                    }
                    else
                        showInvalid(elm)

                }
                return (
                    <div className="card">
                        <h5 className="card-header">Remote Server</h5>
                        <div className="card-body">
                            <form>
                                <div className="form-group">
                                    <label htmlFor="address">Address</label>
                                    <input type="text" className="form-control" name="address" placeholder="IP or hostname" defaultValue={config.address} onChange={fieldChanged} />
                                </div>
                                <div className="form-group">
                                    <label htmlFor="port">Port</label>
                                    <input type="number" className="form-control" name="port" min="0" max="65535" placeholder="9222" defaultValue={config.port} onChange={fieldChanged} />
                                    <div className="invalid-feedback">
                                        Valid port numbers are from 0 (reserved) up to 65535 
                                    </div>
                                </div>
                                <div className="form-group">
                                    <label htmlFor="user">User</label>
                                    <input type="text" className="form-control" name="user" placeholder="john" defaultValue={config.user} onChange={fieldChanged} />
                                </div>
                                <div className="form-group">
                                    <label htmlFor="pass">Pass</label>
                                    <input type="password" className="form-control" name="pass" placeholder="toor" defaultValue={config.pass} onChange={fieldChanged} />
                                </div>
                            </form>
                        </div>
                    </div>
                )
            }
            }
        </DataConsumer>
    )
}

export default Settings