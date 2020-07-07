import React, {useState} from 'react';

import Menu from "./Menu";
import RecordingsList from "./RecordingsList";
import Configuration from "./Configuration";
import {allocateId, getRecordingById} from  '../Utils/LocalStorageUtils.js'
import Recording from "./Recording";
import moment from "moment";
import '../css/App.css';

import 'bootstrap/dist/css/bootstrap.min.css'
import 'popper.js/dist/popper.min.js'
import 'jquery/dist/jquery.min.js'
import 'bootstrap/dist/js/bootstrap.js'
import '@fortawesome/fontawesome-free/css/all.min.css'
import '@fortawesome/fontawesome-free/js/all.min.js'


const App = () => {
   // localStorage.clear()
    const [activeTab, setActiveTab] = useState('My recordings')
    const [recording, setRecording] = useState(false)

    if(!recording) {
        return (
            <>
            <Menu setActiveTab={setActiveTab}/> {
                activeTab === 'My recordings' ? <RecordingsList setRecording={setRecording}/>
                                              : <Configuration/>
            }
            </>
        )
    }
    else {
        return (
            <Recording recordingInfo={getRecordingInfo(recording)} />
        )
    }
}

function getRecordingInfo(recording) {
    if(recording === 'new')
        return createRecording()
    else
        return getRecordingById(recording)

}

function createRecording() {
    const id = allocateId()
    return {
        id: id,
        thumbnail: {
            name: `Untitled ${id}`,
            created: moment(),
            updated: 'never',
            websites: []
        },
        actions: [],
        conf: null
    }
}

export default App;
