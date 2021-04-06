import React from 'react';
import { useServices } from './../../common/services';
import DataLoader from '../../common/components/DataLoader';
import { Paper } from '@material-ui/core';
import BikesList, {
  BikeActionsForBike,
} from '../../common/components/BikesList';
import usePromise from '../../common/hooks/usePromise';
import useRefresh from './../../common/hooks/useRefresh';

const RentalsTab = () => {
  const [refreshBikesState, refreshBikes] = useRefresh();
  const data = usePromise(useServices().getRentedBikes, [refreshBikesState]);
  const returnBike = useServices().returnBike;

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
        returnBike(bikeId).then(() => refreshBikes());
      },
    },
  ];

  return (
    <Paper>
      <DataLoader data={data}>
        {bikes => <BikesList bikes={bikes} bikeActions={bikeActions} />}
      </DataLoader>
    </Paper>
  );
};

export default RentalsTab;
