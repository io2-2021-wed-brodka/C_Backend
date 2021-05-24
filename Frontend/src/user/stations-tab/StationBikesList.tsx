import React from 'react';
import { useServices } from './../../common/services';
import DataLoader from '../../common/components/DataLoader';
import BikesList, {
  BikeActionsForBike,
} from '../../common/components/BikesList';
import usePromise from '../../common/hooks/usePromise';
import SnackBar from '../../common/components/SnackBar';
import { useSnackbar } from '../../common/hooks/useSnackbar';
import useRefresh from '../../common/hooks/useRefresh';

type Props = {
  stationId: string;
};

const StationBikesList = ({ stationId }: Props) => {
  const [refreshBikesState, refreshBikes] = useRefresh();
  const getBikesOnStation = useServices().getBikesOnStation;
  const rentBike = useServices().rentBike;
  const reserveBike = useServices().reserveBike;
  const snackbar = useSnackbar();
  const data = usePromise(() => getBikesOnStation(stationId), [
    stationId,
    refreshBikesState,
  ]);

  const bikeActions: BikeActionsForBike = ({ id }) => [
    {
      label: 'Reserve',
      type: 'secondary',
      onClick: () => {
        reserveBike(id)
          .then(() => {
            refreshBikes();
            snackbar.open(`Reserved bike #${id}`);
          })
          .catch(err => snackbar.open(err.message));
      },
      id: `reserve-${id}`,
    },
    {
      label: 'Rent',
      type: 'primary',
      onClick: () => {
        rentBike(id)
          .then(() => {
            refreshBikes();
            snackbar.open(`Rented bike #${id}`);
          })
          .catch(err => snackbar.open(err.message));
      },
      id: `rent-${id}`,
    },
  ];

  return (
    <>
      <DataLoader data={data}>
        {bikes => <BikesList bikes={bikes} bikeActions={bikeActions} />}
      </DataLoader>
      <SnackBar {...snackbar.props} />
    </>
  );
};

export default StationBikesList;
