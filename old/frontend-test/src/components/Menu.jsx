import React, {useState, useContext} from 'react'
import Header from './Header'
import TabNav from './TabNav'
import RecordingsList from './RecordingsList';
import Settings from './Settings'

const Menu = () => {
  const [activeTab, setActiveTab] = useState('My recordings')
    return (
        <>
      <Header />
      <TabNav setActiveTab={setActiveTab} />
      { activeTab === 'My recordings' ? <RecordingsList />
                                      : <Settings />
      }
      </>
    )
}

export default Menu