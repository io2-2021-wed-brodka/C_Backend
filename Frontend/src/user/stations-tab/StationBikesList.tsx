import React from 'react';
import { useServices } from './../../common/services';
import DataLoader from '../../common/components/DataLoader';
import BikesList from './../../common/components/BikesList';

type Props = {
  stationId: string;
};

const StationBikesList = ({ stationId }: Props) => {
  const data = useServices().useBikesOnStation(stationId);

  return (
    <DataLoader data={data}>{bikes => <BikesList bikes={bikes} />}</DataLoader>
  );
};

export default StationBikesList;
