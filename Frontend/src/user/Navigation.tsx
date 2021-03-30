import React from 'react';
import { createStyles, makeStyles, Paper, Tab, Tabs } from '@material-ui/core';
import { useHistory } from 'react-router-dom';

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
  const [value, setValue] = React.useState(0);
  const history = useHistory();

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const handleChange: any = (event: never, newValue: number) => {
    setValue(newValue);
    history.push(tabs[newValue].url);
  };

  return [value, handleChange];
};

const Navigation = () => {
  const classes = useStyles();
  const [tab, handleTabChange] = useNavigation();

  return (
    <Paper className={classes.root}>
      <Tabs
        value={tab}
        onChange={handleTabChange}
        indicatorColor="primary"
        textColor="primary"
        centered
      >
        {tabs.map(({ name }) => (
          <Tab label={name} key={name}></Tab>
        ))}
      </Tabs>
    </Paper>
  );
};

export default Navigation;
