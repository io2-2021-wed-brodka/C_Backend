import React from 'react';
import './UserApp.css';
import { createMuiTheme, createStyles, makeStyles, Paper, Tab, Tabs, Theme, ThemeProvider } from '@material-ui/core';
import { green, pink } from '@material-ui/core/colors';
import ApplicationBar from './ApplicationBar';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      flexGrow: 1,
    },
    menuButton: {
      marginRight: theme.spacing(2),
    },
    title: {
      flexGrow: 1,
    },
  }),
);

const theme = createMuiTheme({
  palette: {
    primary: pink,
    secondary: green,
  },
});

const UserApp = (): JSX.Element => {
  const classes = useStyles();

  const [value, setValue] = React.useState(0);

  const handleChange = (_event: React.ChangeEvent<unknown>, newValue: number) => {
    setValue(newValue);
  };

  return (
    <ThemeProvider theme={theme}>
      <div className={classes.root}>
        <ApplicationBar />
        <Paper className={classes.root}>
          <Tabs value={value} onChange={handleChange} indicatorColor="primary" textColor="primary" centered>
            <Tab label="Item One" />
            <Tab label="Item Two" />
            <Tab label="Item Three" />
          </Tabs>
        </Paper>
      </div>
    </ThemeProvider>
  );
};

export default UserApp;
