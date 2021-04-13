import React from 'react';
import { useServices } from '../../common/services';
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
  refresh?: unknown;
};

const StationBikesList = ({ stationId, refresh }: Props) => {
  const getBikesOnStation = useServices().getBikesOnStation;
  const removeBike = useServices().removeBike;
  const snackbar = useSnackbar();
  const [internalRefreshState, internalRefresh] = useRefresh();
  const data = usePromise(() => getBikesOnStation(stationId), [
    stationId,
    refresh,
    internalRefreshState,
  ]);

  const bikeActions: BikeActionsForBike = bikeId => [
    {
      label: 'Block',
      type: 'secondary',
      onClick: () => {
        console.log(bikeId);
      },
    },
    {
      label: 'Remove',
      type: 'primary',
      onClick: () => {
        removeBike(bikeId)
          .then(() => internalRefresh())
          .catch(err => snackbar.open(err.message));
      },
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
