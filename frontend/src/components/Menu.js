import React from "react";

import '../css/Menu.css'

const Menu = ({ setActiveTab }) => {

    const changeActiveTab = e => setActiveTab(e.currentTarget.textContent)

    return (
        <header>
            <h1>Puppeteer recorder</h1>
            <nav>
                <div className="nav nav-tabs" role="tablist">
                    <a className="nav-item nav-link active"  data-toggle="tab" href="list" role="tab" aria-selected="true" onClick={changeActiveTab}>My recordings</a>
                    <a className="nav-item nav-link" data-toggle="tab" href="data-conf" role="tab" aria-selected="false" onClick={changeActiveTab}>Default config</a>
                </div>
            </nav>
        </header>
    )
}
export default Menu