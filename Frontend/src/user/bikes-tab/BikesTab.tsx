import React from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import usePromise from '../../common/hooks/usePromise';
import useRefresh from '../../common/hooks/useRefresh';
import BikesList, {
  BikeActionsForBike,
} from '../../common/components/BikesList';
import { BikeStatus } from '../../common/api/models/bike';
import { useSnackbar } from '../../common/hooks/useSnackbar';
import SnackBar from '../../common/components/SnackBar';
import { Paper, Typography } from '@material-ui/core';

const BikesTab = () => {
  const [refreshState, refresh] = useRefresh();
  const getBikes = useServices().getBikes;
  const blockBike = useServices().blockBike;
  const unblockBike = useServices().unblockBike;
  const data = usePromise(getBikes, [refreshState]);
  const snackbar = useSnackbar();

  const onBlockBike = (id: string) => {
    blockBike(id)
      .then(() => refresh())
      .catch(err => snackbar.open(err.message));
  };

  const onUnblockBike = (id: string) => {
    unblockBike(id)
      .then(() => refresh())
      .catch(err => snackbar.open(err.message));
  };

  const bikeActions: BikeActionsForBike = ({ id, status }) => [
    {
      label: status === BikeStatus.Blocked ? 'Unblock' : 'Block',
      type: 'secondary',
      onClick: () =>
        status === BikeStatus.Blocked ? onUnblockBike(id) : onBlockBike(id),
      id: status === BikeStatus.Blocked ? `unblock-${id}` : `block-${id}`,
    },
  ];

  return (
    <>
      <Typography variant="h4" id="bikes-header">
        Bikes
      </Typography>

      <Paper>
        <DataLoader data={data}>
          {bikes => (
            <BikesList
              bikes={bikes}
              bikeActions={bikeActions}
              showStatus={true}
              showLocation={true}
            />
          )}
        </DataLoader>
        <SnackBar {...snackbar.props} />
      </Paper>
    </>
  );
};

export default BikesTab;
