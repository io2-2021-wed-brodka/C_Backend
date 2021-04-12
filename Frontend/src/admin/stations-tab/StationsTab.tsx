import React from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import usePromise from '../../common/hooks/usePromise';
import Station from './Station';

const StationsTab = () => {
  const data = usePromise(useServices().getStations);

  return (
    <DataLoader data={data}>
      {stations =>
        stations.map(station => <Station key={station.id} {...station} />)
      }
    </DataLoader>
  );
};

export default StationsTab;
