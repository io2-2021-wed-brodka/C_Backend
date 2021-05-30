import React from 'react';
import {
  AppBar,
  Container,
  createStyles,
  makeStyles,
  Theme,
  ThemeProvider,
} from '@material-ui/core';
import { BrowserRouter, Switch, Route, Redirect } from 'react-router-dom';
import ApplicationBar from './ApplicationBar';
import Navigation from './Navigation';
import './UserApp.css';
import StationsTab from './stations-tab/StationsTab';
import { services, ServicesContext, useServices } from '../common/services';
import RentalsTab from './rentals-tab/RentalsTab';
import LoginPage from '../common/components/LoginPage';
import { getTokenFromLocalStorage } from '../common/authentication/token-functions';
import pinkTheme from '../common/theme';
import RegistrationPage from './registration/RegistrationPage';
import ReservationsTab from './reservations-tab/ReservationsTab';
import BikesTab from './bikes-tab/BikesTab';
import usePromise from '../common/hooks/usePromise';
import { UserRole } from '../common/api/models/login-response';
import MalfunctionsTab from './malfunctions-tab/MalfunctionsTab';

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

const UserApp = () => {
  const classes = useStyles();
  const role = usePromise(useServices().getRole);

  return (
    <ThemeProvider theme={pinkTheme}>
      <ServicesContext.Provider value={services}>
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
              <Route path="/signup" component={RegistrationPage} />
              <Route exact path="/">
                <Redirect to={'/stations'} />
              </Route>
              <Route
                path="/"
                render={({ location }) => (
                  <>
                    {!getTokenFromLocalStorage() && <Redirect to={'/login'} />}
                    <AppBar position="sticky" id="user-site-navbar">
                      <ApplicationBar />
                      <Navigation pathname={location.pathname} />
                    </AppBar>
                    <Container maxWidth="md" className={classes.container}>
                      <Route path="/stations" component={StationsTab} />
                      <Route path="/rentals" component={RentalsTab} />
                      <Route path="/reservations" component={ReservationsTab} />
                      {role?.results === UserRole.Tech && (
                        <>
                          <Route path="/bikes" component={BikesTab} />
                          <Route
                            path="/malfunctions"
                            component={MalfunctionsTab}
                          />
                        </>
                      )}
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
