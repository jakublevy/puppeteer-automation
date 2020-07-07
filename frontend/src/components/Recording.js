import React, {useEffect} from 'react'
import {saveRecording} from '../Utils/LocalStorageUtils.js'

const Recording = ({recordingInfo}) => {

    useEffect(() => saveRecording(recordingInfo), [recordingInfo])

    return ( <p>{JSON.stringify(recordingInfo)}</p> )
}
export default Recording