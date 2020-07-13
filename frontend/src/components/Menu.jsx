import React from "react";

import '../css/Menu.css'

const Menu = ({ setActiveTab }) => {
    return (
        <header>
            <h1>Puppeteer recorder</h1>
            <nav>
                <div className="nav nav-tabs" role="tablist">
                    <a className="nav-item nav-link active"  data-toggle="tab" href="list" role="tab" aria-selected="true"
                       onClick={() => setActiveTab('My recordings')}>
                        My <i className="fas fa-video"></i>
                    </a>
                    <a className="nav-item nav-link" data-toggle="tab" href="data-conf" role="tab" aria-selected="false"
                       onClick={() => setActiveTab('Configuration')}>
                        Default <i className="fas fa-cog"></i>
                    </a>
                </div>
            </nav>
        </header>
    )
}
export default Menu