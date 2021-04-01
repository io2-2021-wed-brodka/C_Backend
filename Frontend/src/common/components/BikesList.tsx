import React from 'react';
import Grid from '@material-ui/core/Grid';
import BikeTile from './BikeTile';
import Alert from './../../common/components/Alert';
import { Bike } from '../../common/api/models/bike';

type Props = {
  bikes: Bike[];
  onBikeClick?: (bikeId: string) => void;
};

const BikesList = ({ bikes, onBikeClick }: Props) => {
  const onClick = (bikeId: string) => () => {
    if (onBikeClick) {
      onBikeClick(bikeId);
    }
  };

  const NoBikes = () => (
    <Grid item>
      <Alert severity="info">No bikes here now!</Alert>
    </Grid>
  );

  const Bikes = ({ bikes }: { bikes: Bike[] }) => (
    <>
      {bikes.map(({ id }) => (
        <Grid item key={id}>
          <BikeTile id={id} onClick={onClick(id)} />
        </Grid>
      ))}
    </>
  );

  return (
    <Grid container justify="center" spacing={2}>
      <>
        {!bikes.length && <NoBikes />}
        <Bikes bikes={bikes} />
      </>
    </Grid>
  );
};

export default BikesList;
