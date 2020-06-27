import React, {useContext, useState} from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPlus } from '@fortawesome/free-solid-svg-icons'
import DataContext, {configFilled} from '../dataContext.js'
import AlertBox from './AlertBox'

const RecordingsList = () => {
    const {config, setEditor} = useContext(DataContext)

    const [alertMsg, setAlertMsg] = useState(null)    

    const addNewRecordingStyle = {
        marginBottom: '1rem'
    }    

    const addNewRecording = () => {
       const [filled, missingFields] = configFilled(config)
       if(filled)
            setEditor({cmd: 'new'})
       else 
            setAlertMsg('Head to the Setting and fill in: ' + missingFields.join(', '))
    }

    return (
        <>
        {
            alertMsg !== null ? <AlertBox alertType="alert-danger" msg={alertMsg} setAlertMsg={setAlertMsg} /> : <></>
        }
        <button type="button" className="btn btn-secondary" style={addNewRecordingStyle} onClick={addNewRecording}>
            <FontAwesomeIcon icon={faPlus} />
            &nbsp;Add new recording
        </button>
        <table className="table table-striped table-bordered">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Website</th>
                    <th>Updated</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Login to eshop</td>
                    <td>amazon.com</td>
                    <td>Dec 23 2020 15:35</td>
                </tr>
                <tr>
                    <td>Button press</td>
                    <td>cnn.com</td>
                    <td>Dec 24 2020 15:35</td>
                </tr>
                <tr>
                    <td>Button press</td>
                    <td>cnn.com</td>
                    <td>Dec 24 2020 15:35</td>
                </tr>
            </tbody>
        </table>
    </>
    )

}

export default RecordingsList