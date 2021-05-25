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
import { BikeStatus } from '../../common/api/models/bike';

type Props = {
  stationId: string;
  refresh?: unknown;
};

const StationBikesList = ({ stationId, refresh }: Props) => {
  const getBikesOnStation = useServices().getBikesOnStation;
  const removeBike = useServices().removeBike;
  const blockBike = useServices().blockBike;
  const unblockBike = useServices().unblockBike;
  const snackbar = useSnackbar();
  const [internalRefreshState, internalRefresh] = useRefresh();
  const data = usePromise(() => getBikesOnStation(stationId), [
    stationId,
    refresh,
    internalRefreshState,
  ]);

  const bikeActions: BikeActionsForBike = ({ id, status }) => [
    {
      label: status === BikeStatus.Blocked ? 'Unblock' : 'Block',
      type: 'secondary',
      onClick: () => {
        (status === BikeStatus.Blocked ? unblockBike(id) : blockBike(id))
          .then(() => internalRefresh())
          .catch(err => snackbar.open(err.message));
      },
      id: status === BikeStatus.Blocked ? `unblock-bike-${id}` : `block-bike-${id}`,
    },
    {
      label: 'Remove',
      type: 'primary',
      onClick: () => {
        removeBike(id)
          .then(() => internalRefresh())
          .catch(err => snackbar.open(err.message));
      },
      id: `remove-bike${id}`,
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
