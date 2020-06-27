import React, { useState } from 'react';
import Menu from './Menu'
import Editor from './Editor'
import {fetchConfig, DataProvider} from '../dataContext.js'

import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/popper.js'
import 'jquery/dist/jquery.min.js'
import 'bootstrap/dist/js/bootstrap.js'
import '../css/App.css'

function App() {
  const [editor, setEditor] = useState(null)

  const initialData = fetchConfig()
  const [config, setConfig] = useState(initialData)
  return (
      <div id="container">
        <DataProvider value={{config: config, setConfig: setConfig, editor: editor, setEditor: setEditor}}> {
            editor !== null ? <Editor />
                   : <Menu />
          }
        </DataProvider>
      </div>
  )
}

export default App;
