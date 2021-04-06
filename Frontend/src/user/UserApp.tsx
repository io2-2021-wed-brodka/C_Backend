import React from 'react';
import {
  AppBar,
  Container,
  createMuiTheme,
  createStyles,
  makeStyles,
  Theme,
  ThemeProvider,
} from '@material-ui/core';
import { green, pink } from '@material-ui/core/colors';
import { BrowserRouter, Switch, Route, Redirect } from 'react-router-dom';
import ApplicationBar from './ApplicationBar';
import Navigation from './Navigation';
import './UserApp.css';
import StationsTab from './stations-tab/StationsTab';
import { mockedServices, ServicesContext } from '../common/services';
import RentalsTab from './rentals-tab/RentalsTab';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      flexGrow: 1,
      background: theme.palette.grey[100],
      height: 'auto',
      overflow: 'auto',
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
      <ServicesContext.Provider value={mockedServices}>
        <BrowserRouter>
          <div className={classes.root}>
            <Route
              path="/"
              render={({ location }) => (
                <>
                  <AppBar position="sticky">
                    <ApplicationBar />
                    <Navigation pathname={location.pathname} />
                  </AppBar>
                  <Container maxWidth="md" className={classes.container}>
                    <Switch>
                      <Route path="/stations">
                        <StationsTab />
                      </Route>
                      <Route path="/rentals">
                        <RentalsTab />
                      </Route>
                      <Route exact path="/">
                        <Redirect to={'/stations'} />
                      </Route>
                    </Switch>
                  </Container>
                </>
              )}
            />
          </div>
        </BrowserRouter>
      </ServicesContext.Provider>
    </ThemeProvider>
  );
};

export default UserApp;
