import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import reportWebVitals from './reportWebVitals';
import UserApp from './user/UserApp';
import AdminApp from './admin/AdminApp';
import { isUserApp } from './common/environment';

ReactDOM.render(
  <React.StrictMode>
    {isUserApp() ? <UserApp /> : <AdminApp />}
  </React.StrictMode>,
  document.getElementById('root'),
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
