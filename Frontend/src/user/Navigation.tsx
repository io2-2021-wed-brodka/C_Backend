import React from 'react';
import { createStyles, makeStyles, Paper, Tab, Tabs } from '@material-ui/core';
import { Link, useHistory } from 'react-router-dom';

const useStyles = makeStyles(() =>
  createStyles({
    root: {
      flexGrow: 1,
    },
  }),
);

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
];

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

  return (
    <Paper className={classes.root} elevation={0} square>
      <Tabs
        value={pathname}
        onChange={handleTabChange}
        indicatorColor="primary"
        textColor="primary"
        centered
      >
        {tabs.map(({ name, url }) => (
          <Tab
            label={name}
            key={name}
            component={Link}
            to={url}
            value={url}
            id={name.toLowerCase()}
          ></Tab>
        ))}
      </Tabs>
    </Paper>
  );
};

export default Navigation;
