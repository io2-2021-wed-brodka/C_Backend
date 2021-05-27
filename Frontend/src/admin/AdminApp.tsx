import React from 'react';
import {
  createStyles,
  makeStyles,
  Theme,
  ThemeProvider,
} from '@material-ui/core';
import { BrowserRouter, Switch, Route, Redirect } from 'react-router-dom';
import { services, ServicesContext } from '../common/services';
import LoginPage from '../common/components/LoginPage';
import { getTokenFromLocalStorage } from '../common/authentication/token-functions';
import pinkTheme from '../common/theme';
import Layout from './Layout';
import StationsTab from './stations-tab/StationsTab';

import './AdminApp.css';
import UsersTab from './users-tab/UsersTab';
import TechsTab from './techs-tab/TechsTab';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    rootMobile: {
      flexGrow: 1,
      background: theme.palette.grey[100],
      height: 'auto',
      overflow: 'auto',
    },
  }),
);

const AdminApp = () => {
  const classes = useStyles();
  return (
    <ThemeProvider theme={pinkTheme}>
      <ServicesContext.Provider value={services}>
        <BrowserRouter>
          <Switch>
            <Route path="/login">
              <div className={classes.rootMobile}>
                <LoginPage />
              </div>
            </Route>
            <Route exact path="/">
              <Redirect to={'/stations'} />
            </Route>
            <Route
              path="/"
              render={() => (
                <>
                  {!getTokenFromLocalStorage() && <Redirect to={'/login'} />}
                  <Layout>
                    <Route path="/stations" component={StationsTab} />
                    <Route path="/users" component={UsersTab} />
                    <Route path="/techs" component={TechsTab} />
                  </Layout>
                </>
              )}
            />
          </Switch>
        </BrowserRouter>
      </ServicesContext.Provider>
    </ThemeProvider>
  );
};

export default AdminApp;
