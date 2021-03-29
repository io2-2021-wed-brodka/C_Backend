import React from 'react';
import Alert from '../../common/components/Alert';
import Spinner from '../../common/components/Spinner';
import Station from './Station';
import { useServices } from './../../common/services';

const StationsTab = (): JSX.Element => {
  const { results: stations, error, loading } = useServices().useStations();

  return (
    <>
      {loading && <Spinner />}
      {error && <Alert severity="error">Oops! You are offline...</Alert>}
      {stations &&
        stations.map(station => <Station key={station.id} {...station} />)}
    </>
  );
};

export default StationsTab;
