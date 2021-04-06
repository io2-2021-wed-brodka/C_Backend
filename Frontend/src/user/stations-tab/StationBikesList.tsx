import React from 'react';
import { useServices } from './../../common/services';
import DataLoader from '../../common/components/DataLoader';
import BikesList, {
  BikeActionsForBike,
} from '../../common/components/BikesList';

const bikeActions: BikeActionsForBike = bikeId => [
  {
    label: 'Reserve',
    type: 'secondary',
    onClick: () => {
      console.log(bikeId);
    },
  },
  {
    label: 'Rent',
    type: 'primary',
    onClick: () => {
      console.log(bikeId);
    },
  },
];

type Props = {
  stationId: string;
};

const StationBikesList = ({ stationId }: Props) => {
  const data = useServices().useBikesOnStation(stationId);

  return (
    <DataLoader data={data}>
      {bikes => <BikesList bikes={bikes} bikeActions={bikeActions} />}
    </DataLoader>
  );
};

export default StationBikesList;
