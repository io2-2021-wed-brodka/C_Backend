import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import reportWebVitals from './reportWebVitals';
import UserApp from './user/UserApp';
import AdminApp from './admin/AdminApp';

ReactDOM.render(
  <React.StrictMode>
    {process.env.REACT_APP_FOR === 'user' ? <UserApp /> : <AdminApp />}
  </React.StrictMode>,
  document.getElementById('root'),
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
