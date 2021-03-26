import React from 'react';
import useMockedDataFetching from './../../common/mocks/useMockedDataFetching';
import { mockedStations } from './../../common/mocks/stations';
import Alert from '../../common/components/Alert';
import Spinner from '../../common/components/Spinner';
import Station from './Station';

const useDataFetching = useMockedDataFetching(mockedStations);

const StationsTab = (): JSX.Element => {
  const { results: stations, error, loading } = useDataFetching('/stations');

  return (
    <>
      {loading && <Spinner />}
      {error && <Alert severity="error">Oops! You are offline...</Alert>}
      {stations &&
        stations.map((station) => <Station key={station.id} {...station} />)}
    </>
  );
};

export default StationsTab;
