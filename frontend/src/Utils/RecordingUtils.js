import {updateTimestampIfChanged} from "./DateUtils";
import {getDefaultConf} from "./ConfUtils";

const getRecordingById = (id) => getRecordings()[id]

function getRecordings() {
    const recordings = JSON.parse(localStorage.getItem('recordings'))
    if(recordings === null)
        return {}
    return recordings
}

const saveRecordings = (recordings) => localStorage.setItem('recordings', JSON.stringify(recordings))

function getRecordingConfiguration(recording) {
    if(recording.conf === null)
        return { provider: 'default', shown: true, ...getDefaultConf() }
    return { provider: 'custom', ...recording.conf }
}

function getRecordingConfigurationById(recordingId) {
    const recording = getRecordingById(recordingId)
    return getRecordingConfiguration(recording)
}

function updateRecordingConfById(id, conf) {
    const recordings = getRecordings()
    if(conf.provider === 'default')
        recordings[id].conf = null
    else {
        recordings[id].conf = JSON.parse(JSON.stringify(conf))
        delete recordings[id].conf.provider
    }
    saveRecordings(recordings)
}


function allocateId() {
    let id = localStorage.getItem('avail-id')
    if(id === null)
        id = 0
    else
        id = parseInt(id)

    localStorage.setItem('avail-id', (id+1).toString())
    return id
}

function saveRecording(recordingInfo) {
    let recordings = getRecordings()

    if(recordings[recordingInfo.id] !== undefined)
        updateTimestampIfChanged(recordings[recordingInfo.id], recordingInfo)

    recordings[recordingInfo.id] = recordingInfo
    saveRecordings(recordings)
}

function deleteRecodingById(id) {
    let recordings = getRecordings()
    delete recordings[id]
    saveRecordings(recordings)
}

export { getRecordingById, getRecordings, allocateId, saveRecording, deleteRecodingById, getRecordingConfiguration, updateRecordingConfById, getRecordingConfigurationById }