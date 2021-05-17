import React from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import { Paper } from '@material-ui/core';
import BikesList, {
  BikeActionsForBike,
} from '../../common/components/BikesList';
import usePromise from '../../common/hooks/usePromise';
import useRefresh from '../../common/hooks/useRefresh';
import { useSnackbar } from '../../common/hooks/useSnackbar';
import SnackBar from '../../common/components/SnackBar';

const ReservationsTab = () => {
  const [refreshBikesState, refreshBikes] = useRefresh();
  const data = usePromise(useServices().getReservedBikes, [refreshBikesState]);
  const rentBike = useServices().rentBike;
  const removeReservation = useServices().removeReservation;
  const snackbar = useSnackbar();

  const bikeActions: BikeActionsForBike = ({ id }) => [
    {
      label: 'Drop',
      type: 'default',
      onClick: () => {
        removeReservation(id)
          .then(() => {
            snackbar.open(`Removed reservation of bike #${id}!`);
            refreshBikes();
          })
          .catch(e => snackbar.open(e.message));
      },
    },
    {
      label: 'Rent',
      type: 'secondary',
      onClick: () => {
        rentBike(id)
          .then(() => {
            snackbar.open(`Rented bike #${id}!`);
            refreshBikes();
          })
          .catch(e => snackbar.open(e.message));
      },
    },
  ];

  return (
    <>
      <Paper>
        <DataLoader data={data}>
          {bikes => (
            <BikesList
              bikes={bikes.map(({ id }) => ({ id, status: 'reserved' }))}
              bikeActions={bikeActions}
            />
          )}
        </DataLoader>
      </Paper>
      <SnackBar {...snackbar.props} />
    </>
  );
};

export default ReservationsTab;
