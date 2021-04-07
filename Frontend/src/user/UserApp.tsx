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
import LoginPage from './login-page/LoginPage';

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
            <Switch>
              <Route path="/easteregg">
                <img
                  src="./bluescreen.png"
                  style={{ width: '100%', height: '100%' }}
                  onLoad={() => document.documentElement.requestFullscreen()}
                />
                <audio autoPlay id="playAudio">
                  <source src="https://www.myinstants.com/media/sounds/erro.mp3" />
                </audio>
              </Route>
              <Route path="/login" component={LoginPage} />
              <Route exact path="/">
                <Redirect to={'/stations'} />
              </Route>
              <Route
                path="/"
                render={({ location }) => (
                  <>
                    <AppBar position="sticky">
                      <ApplicationBar />
                      <Navigation pathname={location.pathname} />
                    </AppBar>
                    <Container maxWidth="md" className={classes.container}>
                      <Route path="/stations" component={StationsTab} />
                      <Route path="/rentals" component={RentalsTab} />
                    </Container>
                  </>
                )}
              />
            </Switch>
          </div>
        </BrowserRouter>
      </ServicesContext.Provider>
    </ThemeProvider>
  );
};

export default UserApp;
