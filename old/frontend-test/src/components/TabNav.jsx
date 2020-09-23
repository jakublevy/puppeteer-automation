import React from 'react'

const TabNav = ({ setActiveTab }) => {

    const changeActiveTab = e => setActiveTab(e.currentTarget.textContent)

    return (
        <nav>
            <div className="nav nav-tabs" role="tablist" style={navStyle}>
                <a className="nav-item nav-link active"  data-toggle="tab" href="a" role="tab" aria-selected="true" onClick={changeActiveTab}>My recordings</a>
                <a className="nav-item nav-link" data-toggle="tab" href="b" role="tab" aria-selected="false" onClick={changeActiveTab}>Settings</a>
            </div>
        </nav>
    )
}
const navStyle = {
    margin: '0 -0.8rem 1rem -0.8rem',
    paddingLeft: '0.8rem'
}

export default TabNav