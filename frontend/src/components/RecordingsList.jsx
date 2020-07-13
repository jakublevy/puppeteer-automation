import React, {useState} from 'react';
import {getRecordings, deleteRecodingById} from "../Utils/RecordingUtils";
import '../css/RecordingsList.css'
import {formatDate} from "../Utils/DateUtils";
import {useForceUpdate} from "../Utils/HooksUtils";

const RecordingsList = ({setRecordingMode}) => {

    const forceUpdate = useForceUpdate()

    const items = tableItems(setRecordingMode, forceUpdate)
    return (
        <>
        <button type="button" className="btn btn-primary" onClick={() => setRecordingMode('new')}>
            New &nbsp;<i className="fas fa-video"></i>
        </button>
            <h3>List</h3>
            <table className="table table-sm table-hover" cellSpacing="0">
                <thead>
                <tr>
                    <th scope="col">Name</th>
                    <th scope="col">Websites</th>
                    <th scope="col">Created</th>
                    <th scope="col">Last updated</th>
                    <th scope="col">Tools</th>
                </tr>
                </thead>
                <tbody>
                {items}
                </tbody>
            </table>
        </>
    )
}

function tableItems(setRecording, forceUpdate) {
    let items = []
    const recordings = getRecordings()
    for(const r of Object.values(recordings)) {
        items.push (
            <tr key={r.id}>
                <td>{r.thumbnail.name}</td>
                <td>{r.thumbnail.websites}</td>
                <td>{formatDate(r.thumbnail.created)}</td>
                <td>{formatDate(r.thumbnail.updated)}</td>
                <td>
                    <button type="button" className="btn btn-warning" onClick={() => setRecording(r)}>
                        <i className="fas fa-edit"></i>
                    </button>
                    <button type="button" className="btn btn-danger" onClick={() => { deleteRecodingById(r.id); forceUpdate()} }>
                        <i className="fas fa-trash"></i>
                    </button>
                </td>
            </tr>
        )
    }
    return items
}

export default RecordingsList
