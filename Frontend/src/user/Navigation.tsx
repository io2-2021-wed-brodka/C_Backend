import React from 'react';
import { createStyles, makeStyles, Paper, Tab, Tabs } from '@material-ui/core';
import { Link, useHistory } from 'react-router-dom';
import { useServices } from '../common/services';
import usePromise from '../common/hooks/usePromise';
import { UserRole } from '../common/api/models/login-response';

const useStyles = makeStyles(() =>
  createStyles({
    root: {
      flexGrow: 1,
    },
  }),
);

const useNavigation = () => {
  const history = useHistory();

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const handleChange: any = (event: never, newValue: string) => {
    history.push(newValue);
  };

  return handleChange;
};

type Props = {
  pathname: string;
};

const Navigation = ({ pathname }: Props) => {
  const classes = useStyles();
  const handleTabChange = useNavigation();
  const role = usePromise(useServices().getRole);

  const tabs = [
    {
      name: 'Stations',
      url: '/stations',
    },
    {
      name: 'Rentals',
      url: '/rentals',
    },
    {
      name: 'Reservations',
      url: '/reservations',
    },
    {
      name: 'Contact',
      url: '/contact',
    },
    {
      name: 'Bikes',
      url: '/bikes',
      canDisplay: () => role.results == UserRole.Tech,
    },
    {
      name: 'Malfunctions',
      url: '/malfunctions',
      canDisplay: () => role.results == UserRole.Tech,
    },
  ];

  return (
    <Paper className={classes.root} elevation={0} square>
      <Tabs
        value={pathname}
        onChange={handleTabChange}
        indicatorColor="primary"
        textColor="primary"
        centered
      >
        {tabs.map(
          ({ name, url, canDisplay }) =>
            (canDisplay ? canDisplay() : true) && (
              <Tab
                label={name}
                key={name}
                component={Link}
                to={url}
                value={url}
                id={name.toLowerCase()}
              ></Tab>
            ),
        )}
      </Tabs>
    </Paper>
  );
};

export default Navigation;
