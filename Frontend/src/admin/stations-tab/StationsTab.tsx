import React from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import usePromise from '../../common/hooks/usePromise';
import Station from './Station';
import NewStationForm from './NewStationForm';
import useRefresh from './../../common/hooks/useRefresh';
import { useSnackbar } from './../../common/hooks/useSnackbar';
import SnackBar from '../../common/components/SnackBar';

const StationsTab = () => {
  const [refreshState, refresh] = useRefresh();
  const data = usePromise(useServices().getStations, [refreshState]);
  const addStation = useServices().addStation;
  const snackbar = useSnackbar();

  const onAddStation = (name: string) => {
    addStation(name)
      .then(() => refresh())
      .catch(err => snackbar.open(err.message));
  };

  return (
    <>
      <NewStationForm onAdd={onAddStation} />
      <DataLoader data={data}>
        {stations =>
          stations.map(station => <Station key={station.id} {...station} />)
        }
      </DataLoader>
      <SnackBar {...snackbar.props} />
    </>
  );
};

export default StationsTab;
