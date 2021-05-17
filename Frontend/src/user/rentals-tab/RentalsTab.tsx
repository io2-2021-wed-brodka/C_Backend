import React, { useState } from 'react';
import { useServices } from './../../common/services';
import DataLoader from '../../common/components/DataLoader';
import { Paper } from '@material-ui/core';
import BikesList, {
  BikeActionsForBike,
} from '../../common/components/BikesList';
import usePromise from '../../common/hooks/usePromise';
import useRefresh from './../../common/hooks/useRefresh';
import { useSnackbar } from './../../common/hooks/useSnackbar';
import SnackBar from '../../common/components/SnackBar';
import StationsDialog from './StationsDialog';
import { Station } from '../../common/api/models/station';

const RentalsTab = () => {
  const [refreshBikesState, refreshBikes] = useRefresh();
  const data = usePromise(useServices().getRentedBikes, [refreshBikesState]);
  const returnBike = useServices().returnBike;
  const snackbar = useSnackbar();
  const [dialogIsOpen, setDialogIsOpen] = useState(false);
  const [returnedBikeId, setReturnedBikeId] = useState('');

  const handleDialogClose = () => {
    setDialogIsOpen(false);
  };

  const selectStation = (station: Station) => {
    setDialogIsOpen(false);
    returnBike(station.id, returnedBikeId).then(() => {
      refreshBikes();
      snackbar.open(
        `Returned bike #${returnedBikeId} on station ${station.name}`,
      );
    });
  };

  const bikeActions: BikeActionsForBike = ({ id }) => [
    {
      label: 'Report',
      type: 'default',
      onClick: () => {
        alert(id);
      },
    },
    {
      label: 'Return',
      type: 'secondary',
      onClick: () => {
        setReturnedBikeId(id);
        setDialogIsOpen(true);
      },
    },
  ];

  return (
    <>
      <Paper>
        <DataLoader data={data}>
          {bikes => <BikesList bikes={bikes} bikeActions={bikeActions} />}
        </DataLoader>
      </Paper>
      <SnackBar {...snackbar.props} />

      {dialogIsOpen && (
        <StationsDialog
          close={handleDialogClose}
          selectStation={selectStation}
        />
      )}
    </>
  );
};

export default RentalsTab;
