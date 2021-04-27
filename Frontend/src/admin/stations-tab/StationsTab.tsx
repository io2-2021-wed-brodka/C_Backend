import React, { useState } from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import usePromise from '../../common/hooks/usePromise';
import Station from './Station';
import NewStationForm from './NewStationForm';
import useRefresh from './../../common/hooks/useRefresh';
import { useSnackbar } from './../../common/hooks/useSnackbar';
import SnackBar from '../../common/components/SnackBar';
import { Button } from '@material-ui/core';

enum StationFilterType {
  All = 'all',
  Blocked = 'blocked',
  Active = 'active',
}

const StationsTab = () => {
  const [refreshState, refresh] = useRefresh();
  const data = usePromise(useServices().getStations, [refreshState]);
  const addStation = useServices().addStation;
  const snackbar = useSnackbar();
  const [stationFilter, SetStationFilter] = useState(StationFilterType.All);

  const onAddStation = (name: string) => {
    addStation(name)
      .then(() => refresh())
      .catch(err => snackbar.open(err.message));
  };

  return (
    <>
      <Button
        variant="contained"
        color={stationFilter == StationFilterType.All ? 'secondary' : 'primary'}
        onClick={() => SetStationFilter(StationFilterType.All)}
      >
        All
      </Button>
      <Button
        variant="contained"
        color={
          stationFilter == StationFilterType.Active ? 'secondary' : 'primary'
        }
        onClick={() => SetStationFilter(StationFilterType.Active)}
      >
        Active
      </Button>
      <Button
        variant="contained"
        color={
          stationFilter == StationFilterType.Blocked ? 'secondary' : 'primary'
        }
        onClick={() => SetStationFilter(StationFilterType.Blocked)}
      >
        Blocked
      </Button>

      <NewStationForm onAdd={onAddStation} />
      <DataLoader data={data}>
        {stations =>
          stations
            .filter(
              station =>
                station.status == stationFilter ||
                stationFilter == StationFilterType.All,
            )
            .map(station => (
              <Station key={station.id} forceRefresh={refresh} {...station} />
            ))
        }
      </DataLoader>
      <SnackBar {...snackbar.props} />
    </>
  );
};

export default StationsTab;
