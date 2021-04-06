import React from 'react';
import { useServices } from './../../common/services';
import DataLoader from '../../common/components/DataLoader';
import { Paper } from '@material-ui/core';
import BikesList, {
  BikeActionsForBike,
} from '../../common/components/BikesList';
import usePromise from '../../common/hooks/usePromise';
import useRefresh from './../../common/hooks/useRefresh';
import { useSnackbar } from './../../common/hooks/useSnackbar';
import SnackBar from '../../common/components/SnackBarComponent';

const RentalsTab = () => {
  const [refreshBikesState, refreshBikes] = useRefresh();
  const data = usePromise(useServices().getRentedBikes, [refreshBikesState]);
  const returnBike = useServices().returnBike;
  const snackbar = useSnackbar();

  const onReturnBike = (bikeId: string) => () => {
    returnBike(bikeId).then(() => {
      refreshBikes();
      snackbar.open(`Returned bike #${bikeId}`);
    });
  };

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
      onClick: onReturnBike(bikeId),
    },
  ];

  return (
    <Paper>
      <DataLoader data={data}>
        {bikes => <BikesList bikes={bikes} bikeActions={bikeActions} />}
      </DataLoader>
      <SnackBar {...snackbar.props} />
    </Paper>
  );
};

export default RentalsTab;
