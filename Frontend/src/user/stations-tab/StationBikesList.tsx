import React from 'react';
import Grid from '@material-ui/core/Grid';
import BikeTile from './BikeTile';
import Spinner from '../../common/components/Spinner';
import Alert from './../../common/components/Alert';
import { useServices } from './../../common/services';

type Props = {
  stationId: string;
};

const StationBikesList = ({ stationId }: Props) => {
  const { results: bikes, loading } = useServices().useBikesOnStation(
    stationId,
  );

  return (
    <Grid container justify="center" spacing={2}>
      {loading && <Spinner />}
      {bikes && !bikes.length && (
        <Grid item>
          <Alert severity="info">No bikes here now!</Alert>
        </Grid>
      )}
      {bikes &&
        bikes.map(({ id }) => (
          <Grid item key={id}>
            <BikeTile id={id} />
          </Grid>
        ))}
    </Grid>
  );
};

export default StationBikesList;
