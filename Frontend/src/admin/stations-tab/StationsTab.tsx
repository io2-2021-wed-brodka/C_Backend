import React, { useState } from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import usePromise from '../../common/hooks/usePromise';
import Station from './Station';
import NewStationForm from './NewStationForm';
import useRefresh from './../../common/hooks/useRefresh';
import { useSnackbar } from './../../common/hooks/useSnackbar';
import SnackBar from '../../common/components/SnackBar';
import { Button, Typography } from '@material-ui/core';
import { StationStatus } from '../../common/api/models/station';

enum StationStatusExtension {
  All = 'all',
}

type StationFilterType = StationStatus | StationStatusExtension;
const stationFilterType = { ...StationStatus, ...StationStatusExtension };

const StationsTab = () => {
  const [refreshState, refresh] = useRefresh();
  const data = usePromise(useServices().getAllStations, [refreshState]);
  const addStation = useServices().addStation;
  const snackbar = useSnackbar();
  const [stationFilter, setStationFilter] = useState<StationFilterType>(
    stationFilterType.All,
  );

  const onAddStation = (name: string, bikesLimit?: number) => {
    addStation(name, bikesLimit)
      .then(() => refresh())
      .catch(err => snackbar.open(err.message));
  };

  return (
    <>
      <Typography variant="h4" id="stations-header">
        Stations
      </Typography>
      <br />
      <Button
        variant="contained"
        color={stationFilter == stationFilterType.All ? 'secondary' : 'primary'}
        onClick={() => setStationFilter(stationFilterType.All)}
      >
        All
      </Button>
      <Button
        variant="contained"
        color={
          stationFilter == stationFilterType.Active ? 'secondary' : 'primary'
        }
        onClick={() => setStationFilter(stationFilterType.Active)}
      >
        Active
      </Button>
      <Button
        variant="contained"
        color={
          stationFilter == stationFilterType.Blocked ? 'secondary' : 'primary'
        }
        onClick={() => setStationFilter(stationFilterType.Blocked)}
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
                stationFilter == stationFilterType.All,
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
