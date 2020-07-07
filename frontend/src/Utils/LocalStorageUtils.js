const getRecordingById = (id) => getRecordings()[id]

function getRecordings() {
    const recordings = JSON.parse(localStorage.getItem('recordings'))
    if(recordings === null)
        return []
    return recordings
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
    recordings[recordingInfo.id] = recordingInfo
    localStorage.setItem('recordings', JSON.stringify(recordings))
}

export { getRecordingById, getRecordings, allocateId, saveRecording }