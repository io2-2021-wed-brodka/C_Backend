import React from 'react';
import Grid from '@material-ui/core/Grid';
import BikeTile from './BikeTile';
import Alert from './../../common/components/Alert';
import { useServices } from './../../common/services';
import DataLoader from '../../common/components/DataLoader';
import { Bike } from '../../common/api/models/bike';

type Props = {
  stationId: string;
};

const StationBikesList = ({ stationId }: Props) => {
  const data = useServices().useBikesOnStation(stationId);

  const NoBikes = () => (
    <Grid item>
      <Alert severity="info">No bikes here now!</Alert>
    </Grid>
  );

  const Bikes = ({ bikes }: { bikes: Bike[] }) => (
    <>
      {bikes.map(({ id }) => (
        <Grid item key={id}>
          <BikeTile id={id} />
        </Grid>
      ))}
    </>
  );

  return (
    <Grid container justify="center" spacing={2}>
      <DataLoader data={data}>
        {bikes => (
          <>
            {!bikes.length && <NoBikes />}
            <Bikes bikes={bikes} />
          </>
        )}
      </DataLoader>
    </Grid>
  );
};

export default StationBikesList;
