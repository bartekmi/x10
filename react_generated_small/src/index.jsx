// @flow

import React from 'react';
import ReactDOM from 'react-dom';

import './index.css';

// Various external CSS's that need to be specially imported
// I am actually not sure if this is necessary - tried removing it and couldn't
// get things to fail. Scared to remove it because I spent ~2 days on this!
// import "react-datepicker/dist/react-datepicker.css";

import App from './App';

ReactDOM.render(
  <React.StrictMode>
    <App/>
  </React.StrictMode>,
  document.getElementById('root')
);
