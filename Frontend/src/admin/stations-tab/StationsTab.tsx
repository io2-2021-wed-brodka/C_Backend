import React from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import usePromise from '../../common/hooks/usePromise';
import Station from './Station';
import {
  Button,
  createStyles,
  makeStyles,
  TextField,
  Theme,
} from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    topBar: {
      paddingBottom: theme.spacing(2),
      display: 'flex',
      alignItems: 'flex-end',
    },
    input: {
      marginRight: theme.spacing(1),
    },
    newStationButton: {
      marginRight: theme.spacing(1),
    },
  }),
);

const StationsTab = () => {
  const classes = useStyles();

  const data = usePromise(useServices().getStations);

  return (
    <>
      <div className={classes.topBar}>
        <TextField label="New station's name" className={classes.input} />
        <Button variant="contained" color={'secondary'}>
          Add station
        </Button>
      </div>

      <DataLoader data={data}>
        {stations =>
          stations.map(station => <Station key={station.id} {...station} />)
        }
      </DataLoader>
    </>
  );
};

export default StationsTab;
