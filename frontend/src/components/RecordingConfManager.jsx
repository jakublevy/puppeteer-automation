import React, {useEffect, useLayoutEffect, useRef} from 'react'
import {getRecordingConfiguration, saveRecording, updateRecordingConfById} from "../Utils/RecordingUtils";
import _ from 'lodash'
import {useForceUpdate} from "../Utils/HooksUtils";
import Configuration from "./Configuration";

const RecordingConfManager = ({conf, recordingId, setRecordingConf}) => {

    const forceUpdate = useForceUpdate()

    const confChange = (name, value) => {
        _.set(conf, name, value)
        forceUpdate()
        setRecordingConf(JSON.parse(JSON.stringify(conf)))

    }

    useEffect(() => {
        if(conf.provider === 'custom')
            setSwitchState(conf.shown)

    }, [conf])

    function showConfCheckedChanged(e) {
        setSwitchState(e.target.checked)
        confChange('shown', e.target.checked)
    }

    function addCustomConf() {
      //  setSwitchState(true)
        confChange('provider', 'custom')
    }

    function setSwitchState(state = undefined) {
        const sw = document.getElementById('showConf')
        state = state !== undefined ? state : !sw.checked
        if(state) {
            if(!sw.checked)
                sw.click()

            document.getElementById('showConfLabel').innerText = 'Shown'
        }
        else {
            if(sw.checked)
                sw.click()

            document.getElementById('showConfLabel').innerText = 'Hidden'
        }
    }

    return (
        <>
            {conf.provider === 'default' ?
             <>
                <p key={conf.provider}>No custom <i className="fas fa-cog"></i> created, using default. <button type="button" className="btn btn-success" onClick={addCustomConf}>
                    <i className="fas fa-plus-circle"></i></button></p>
            </> :
             <>
                <p key={conf.provider}>
                    Using custom <i className="fas fa-cog"></i>. &nbsp;
                    <button type="button" className="btn btn-danger" onClick={() => confChange('provider', 'default')}>
                        <i className="fas fa-minus-circle"></i>
                    </button>
                    <span style={{display: 'inline-block'}} className="custom-control custom-switch">
                        <input type="checkbox" className="custom-control-input" id="showConf" onChange={showConfCheckedChanged} />
                        <label id="showConfLabel" className="custom-control-label" htmlFor="showConf">Shown</label>
                    </span>
                </p>
                 {conf.shown ? <Configuration conf={conf} setConf={setRecordingConf}/> : <></>
                 }
             </>}
        </>
    )
}

export default RecordingConfManager