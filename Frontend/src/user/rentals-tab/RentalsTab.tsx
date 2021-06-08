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
import MalfunctionDialog from './MalfunctionDialog';

const RentalsTab = () => {
  const [refreshBikesState, refreshBikes] = useRefresh();
  const data = usePromise(useServices().getRentedBikes, [refreshBikesState]);
  const returnBike = useServices().returnBike;
  const addMalfunction = useServices().addMalfunction;
  const snackbar = useSnackbar();
  const [stationsDialogIsOpen, setStationsDialogIsOpen] = useState(false);
  const [malfunctionDialogIsOpen, setMalfunctionDialogIsOpen] = useState(false);
  const [returnedBikeId, setReturnedBikeId] = useState('');
  const [reportedBikeId, setReportedBikeId] = useState('');
  const [malfunctionDescription, setMalfunctionDescription] = useState('');

  const handleStationsDialogClose = () => {
    setStationsDialogIsOpen(false);
    setMalfunctionDescription('');
  };

  const handleMalfunctionDialogClose = () => {
    setMalfunctionDialogIsOpen(false);
    setMalfunctionDescription('');
  };

  const selectStation = async (station: Station) => {
    setStationsDialogIsOpen(false);

    try {
      if (malfunctionDescription) {
        await addMalfunction(reportedBikeId, malfunctionDescription);
        snackbar.open(
          `Returned bike #${returnedBikeId} on station ${station.name} and reported malfunction`,
        );
      } else {
        snackbar.open(
          `Returned bike #${returnedBikeId} on station ${station.name}`,
        );
      }

      await returnBike(station.id, returnedBikeId);

      snackbar.open(
        `Returned bike #${returnedBikeId} on station ${station.name}${
          malfunctionDescription && ' and reported malfunction'
        }`,
      );

      setMalfunctionDescription('');
      refreshBikes();
    } catch (err) {
      snackbar.open(err.message);
    }
  };

  const onReportMalfunction = (description: string) => {
    setMalfunctionDialogIsOpen(false);
    setMalfunctionDescription(description);
    setReturnedBikeId(reportedBikeId);
    setStationsDialogIsOpen(true);
  };

  const bikeActions: BikeActionsForBike = ({ id }) => [
    {
      label: 'Report',
      type: 'default',
      onClick: () => {
        setReportedBikeId(id);
        setMalfunctionDialogIsOpen(true);
      },
      id: `report-${id}`,
    },
    {
      label: 'Return',
      type: 'secondary',
      onClick: () => {
        setReturnedBikeId(id);
        setStationsDialogIsOpen(true);
      },
      id: `return-${id}`,
    },
  ];

  return (
    <>
      <Paper>
        <DataLoader data={data}>
          {bikes => (
            <BikesList
              bikes={bikes}
              bikeActions={bikeActions}
              showStatus={false}
              showLocation={false}
            />
          )}
        </DataLoader>
      </Paper>
      <SnackBar {...snackbar.props} />

      {stationsDialogIsOpen && (
        <StationsDialog
          close={handleStationsDialogClose}
          selectStation={selectStation}
        />
      )}
      {malfunctionDialogIsOpen && (
        <MalfunctionDialog
          bikeId={reportedBikeId}
          close={handleMalfunctionDialogClose}
          onReportMalfunction={onReportMalfunction}
        />
      )}
    </>
  );
};

export default RentalsTab;
