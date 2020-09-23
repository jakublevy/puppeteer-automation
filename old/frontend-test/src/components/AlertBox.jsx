import React from 'react'

const AlertBox = ({alertType, msg, setAlertMsg}) => {
    debugger
    return <div className={"alert alert-dismissible fade show " + alertType} role="alert">
    {msg}
    <button type="button" className="close" data-dismiss="alert" aria-label="Close" onClick={() => setAlertMsg(null)}>
            <span aria-hidden="true">&times;</span>
        </button>
    </div>
}

export default AlertBox