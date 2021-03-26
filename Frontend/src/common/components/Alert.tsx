import React from 'react';
import MuiAlert, { AlertProps } from '@material-ui/lab/Alert';

const Alert = (props: AlertProps): JSX.Element => {
  return <MuiAlert elevation={6} variant="filled" {...props} />;
};
export default Alert;
