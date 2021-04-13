import React from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import BikesList, {
  BikeActionsForBike,
} from '../../common/components/BikesList';
import usePromise from '../../common/hooks/usePromise';
import SnackBar from '../../common/components/SnackBar';
import { useSnackbar } from '../../common/hooks/useSnackbar';

type Props = {
  stationId: string;
  refresh?: unknown;
};

const StationBikesList = ({ stationId, refresh }: Props) => {
  const getBikesOnStation = useServices().getBikesOnStation;
  const snackbar = useSnackbar();
  const data = usePromise(() => getBikesOnStation(stationId), [
    stationId,
    refresh,
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
        console.log(bikeId);
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
