import React from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import usePromise from '../../common/hooks/usePromise';
import Station from './Station';
import { Button, createStyles, makeStyles, Theme } from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    topBar: {
      paddingBottom: theme.spacing(2),
    },
  }),
);

const StationsTab = () => {
  const classes = useStyles();

  const data = usePromise(useServices().getStations);

  return (
    <>
      <div className={classes.topBar}>
        <Button variant="contained" color={'secondary'}>
          New station
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
