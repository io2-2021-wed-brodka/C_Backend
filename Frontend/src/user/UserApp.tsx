import React from 'react';
import {
  Container,
  createMuiTheme,
  createStyles,
  makeStyles,
  Theme,
  ThemeProvider,
} from '@material-ui/core';
import { green, pink } from '@material-ui/core/colors';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import ApplicationBar from './ApplicationBar';
import Navigation from './Navigation';
import './UserApp.css';
import StationsTab from './stations-tab/StationsTab';
import { ServicesContext } from '../common/services';
import { services } from './../common/services';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      flexGrow: 1,
      background: theme.palette.grey[100],
    },
    menuButton: {
      marginRight: theme.spacing(2),
    },
    title: {
      flexGrow: 1,
    },
    container: {
      paddingTop: theme.spacing(2),
      paddingBottom: theme.spacing(2),
    },
  }),
);

const theme = createMuiTheme({
  palette: {
    primary: pink,
    secondary: green,
  },
});

const UserApp = () => {
  const classes = useStyles();

  return (
    <ThemeProvider theme={theme}>
      <Router>
        <ServicesContext.Provider value={services}>
          <div className={classes.root}>
            <ApplicationBar />
            <Navigation />
            <Container maxWidth="md" className={classes.container}>
              <Switch>
                <Route path="/stations">
                  <StationsTab />
                </Route>
                <Route path="/">
                  <StationsTab />
                </Route>
              </Switch>
            </Container>
          </div>
        </ServicesContext.Provider>
      </Router>
    </ThemeProvider>
  );
};

export default UserApp;
