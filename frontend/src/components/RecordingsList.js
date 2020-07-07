import React from 'react';
import {getRecordings} from "../Utils/LocalStorageUtils";
import '../css/RecordingsList.css'
import {formatDate} from "../Utils/DateUtils";

const RecordingsList = ({setRecording}) => {
    const items=  tableItems()
    return (
        <>
        <button type="button" className="btn btn-primary" onClick={() => setRecording('new')}>
            <i className="fas fa-plus-circle"></i>&nbsp;
            Add new recoding
        </button>
            <h3>List</h3>
            <table className="table table-sm table-hover" cellSpacing="0">
                <thead>
                <tr>
                    <th scope="col">Name</th>
                    <th scope="col">Websites</th>
                    <th scope="col">Created</th>
                    <th scope="col">Last updated</th>
                </tr>
                </thead>
                <tbody>
                {items}
                </tbody>
            </table>
        </>
    )
}

function tableItems() {
    let items = []
    const recordings = getRecordings()
    for(const r of recordings) {
        items.push (
            <tr key={r.id} data-id={r.id}>
                <td>{r.thumbnail.name}</td>
                <td>{r.thumbnail.websites}</td>
                <td>{formatDate(r.thumbnail.created)}</td>
                <td>{formatDate(r.thumbnail.updated)}</td>
            </tr>
        )
    }
    return items
}

export default RecordingsList
