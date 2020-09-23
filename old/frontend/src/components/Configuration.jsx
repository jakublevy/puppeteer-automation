import React, {useEffect} from 'react';
import {useForceUpdate} from "../Utils/HooksUtils";
import {saveDefaultConf} from "../Utils/ConfUtils";
import _ from 'lodash'


const Configuration = ({conf, setConf}) => {

    const forceUpdate = useForceUpdate()

    useEffect(() => {
        if(conf.startup === 'launch')
            document.getElementById('launchConfig').checked = true
        else if(conf.startup === 'connect')
            document.getElementById('connectConfig').checked = true
    }, [conf])

    const confChange = (name, value) => {
        // _.set(conf, name, value)
        // saveDefaultConf(conf)
        // forceUpdate()

        _.set(conf, name, value)
        forceUpdate()
        setConf(JSON.parse(JSON.stringify(conf)))
    }

    const createForm = () => {
        let form = []
        if(conf.startup === 'launch') {
            form.push(
                <form key={conf.startup}>
                    <div className="form-group">
                        <label htmlFor="path">Path</label>
                        <input type="text" id="path" className="form-control" placeholder="Path to Chromium executable" defaultValue={conf.launchOptions.path}
                               onInput={(e) => confChange('launchOptions.path', e.target.value)}
                        //       key={conf.launchOptions.path}
                        />

                    </div>
                </form>
            )
        }
        else if(conf.startup === 'connect') {
            form.push(
                <form key={conf.startup}>
                    <div className="form-group">
                        <label htmlFor="endPoint">Address</label>
                        <input type="text" id="endPoint" className="form-control" placeholder="IP or hostname" defaultValue={conf.connectOptions.endPoint}
                               onInput={(e) => confChange('connectOptions.endPoint', e.target.value)}
//                               key={conf.connectOptions.endPoint}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="port">Port</label>
                        <input type="number" className="form-control" id="port" min="0" max="65535" placeholder="Remote debugging port" defaultValue={conf.connectOptions.port}
                               onInput={(e) => confChange('connectOptions.port', e.target.value)}
                        //       key={conf.connectOptions.port}
                        />
                        <div className="invalid-feedback">
                            Valid port numbers are from 0 (reserved) up to 65535
                        </div>
                    </div>
                    <div className="form-group">
                        <label htmlFor="user">User</label>
                        <input type="text" className="form-control" id="user" placeholder="Remote username" defaultValue={conf.connectOptions.user}
                               onInput={(e) => confChange('connectOptions.user', e.target.value)}
                        //       key={conf.connectOptions.user}
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="pass">Pass</label>
                        <input type="password" className="form-control" id="pass" placeholder="Remote user's password" defaultValue={conf.connectOptions.pass}
                               onInput={(e) => confChange('connectOptions.pass', e.target.value)}
                      //         key={conf.connectOptions.pass}
                        />
                    </div>
                </form>
            )
        }
        return form
    }

    const fields = createForm()

    return (
        <>
            <div className="card">
                <h5 className="card-header">
                    <div className="btn-group btn-group-toggle" data-toggle="buttons">
                        <label className={`btn btn-light ${conf.startup === 'launch' ? 'active' : ''}`}>
                            <input type="radio" name="options" id="launchConfig" autoComplete="off"
                                   onClick={() => confChange('startup','launch')}
                            />Launch
                        </label>
                        <label className={`btn btn-light ${conf.startup === 'connect' ? 'active' : ''}`}>
                            <input type="radio" name="options" id="connectConfig" autoComplete="off"
                                   onClick={() => confChange('startup','connect')}
                            />Connect
                        </label>
                    </div>
                </h5>
                <div className="card-body">
                    {fields}
                </div>
            </div>



        </>
    )
}

export default Configuration
