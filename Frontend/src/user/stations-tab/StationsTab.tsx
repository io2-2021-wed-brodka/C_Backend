import React from 'react';
import Station from './Station';
import { useServices } from './../../common/services';
import DataLoader from '../../common/components/DataLoader';

const StationsTab = () => {
  const data = useServices().useStations();

  return (
    <DataLoader data={data}>
      {stations =>
        stations.map(station => <Station key={station.id} {...station} />)
      }
    </DataLoader>
  );
};

export default StationsTab;
