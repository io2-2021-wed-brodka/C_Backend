import React from 'react';
import { useServices } from './../../common/services';
import DataLoader from '../../common/components/DataLoader';
import { Paper } from '@material-ui/core';
import BikesList, {
  BikeActionsForBike,
} from '../../common/components/BikesList';

const bikeActions: BikeActionsForBike = bikeId => [
  {
    label: 'Report',
    type: 'default',
    onClick: () => {
      alert(bikeId);
    },
  },
  {
    label: 'Return',
    type: 'secondary',
    onClick: () => {
      alert(bikeId);
    },
  },
];

const RentalsTab = () => {
  const data = useServices().useRentedBikes();

  return (
    <Paper>
      <DataLoader data={data}>
        {bikes => <BikesList bikes={bikes} bikeActions={bikeActions} />}
      </DataLoader>
    </Paper>
  );
};

export default RentalsTab;
