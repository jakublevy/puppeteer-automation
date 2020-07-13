import React, {useEffect, useState} from 'react'
import {saveRecording} from '../Utils/RecordingUtils.js'
import '../css/Recording.css'
import {useForceUpdate} from "../Utils/HooksUtils";
import _ from 'lodash'
import {getRecordingConfiguration, getRecordings} from "../Utils/RecordingUtils";
import RecordingConfManager from "./RecordingConfManager";

const Recording = ({recordingInfo, setRecordingMode}) => {

    const forceUpdate = useForceUpdate()

    useEffect(() => saveRecording(recordingInfo), [recordingInfo])

    const recordingChange = (name, value) => {
        _.set(recordingInfo, name, value)
        saveRecording(recordingInfo)
        forceUpdate()
    }

    const [recordingConf, setRecordingConf] = useState(getRecordingConfiguration(recordingInfo))
    useEffect(() => {
        if(recordingConf.provider === 'default')
            recordingInfo.conf = null
        else {
            recordingInfo.conf = JSON.parse(JSON.stringify(recordingConf))
            delete recordingInfo.provider
        }
        saveRecording(recordingInfo)
    }, [recordingConf])

    return (
        <>
            <button className="btn btn-light" onClick={() => setRecordingMode(false)}>
                <i className="fas fa-chevron-circle-left"></i>
            </button>
            <input type="text" id="name" className="form-control" defaultValue={recordingInfo.thumbnail.name}
                  onInput={(e) => recordingChange('thumbnail.name', e.target.value)}>
            </input>
            <RecordingConfManager recordingId={recordingInfo.id} conf={recordingConf} setRecordingConf={setRecordingConf} />

            <table className="table table-sm table-hover" cellSpacing="0">
                <thead>
                <tr>
                    <th scope="col">Event</th>
                    <th scope="col">Locator</th>
                    <th scope="col">Selector</th>
                    <th scope="col">JSON</th>
                </tr>
                </thead>
                <tbody>
                </tbody>
            </table>

        </>
    )
}

export default Recording