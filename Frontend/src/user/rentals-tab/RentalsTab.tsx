import React from 'react';
import { useServices } from './../../common/services';
import DataLoader from '../../common/components/DataLoader';
import BikesList from '../../common/components/BikesList';
import { Card, CardContent } from '@material-ui/core';

const RentalsTab = () => {
  const data = useServices().useRentedBikes();

  return (
    <Card>
      <CardContent>
        <DataLoader data={data}>
          {bikes => (
            <BikesList
              bikes={bikes}
              onBikeClick={bikeId => () => console.log(bikeId)}
            />
          )}
        </DataLoader>
      </CardContent>
    </Card>
  );
};

export default RentalsTab;
