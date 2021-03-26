import React from 'react';
import Grid from '@material-ui/core/Grid';
import BikeTile from './BikeTile';
import useMockedDataFetching from '../../common/mocks/useMockedDataFetching';
import { mockedBikesByStations } from './../../common/mocks/bikes';
import Spinner from '../../common/components/Spinner';
import Alert from './../../common/components/Alert';

const useDataFetching = (url: string) => {
  return useMockedDataFetching(mockedBikesByStations[url.split('/')[2]])(''); // use mocks
};

type Props = {
  stationId: string;
};

const StationBikesList = ({ stationId }: Props): JSX.Element => {
  const { results: bikes, loading } = useDataFetching(
    `/stations/${stationId}/bikes`,
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
