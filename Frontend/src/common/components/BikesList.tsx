import React from 'react';
import {
  createStyles,
  Button,
  List,
  ListItem,
  ListItemSecondaryAction,
  makeStyles,
  Theme,
  Typography,
  Chip,
} from '@material-ui/core';
import DirectionsBikeIcon from '@material-ui/icons/DirectionsBike';
import { Bike, BikeStatus } from '../api/models/bike';
import ListItemIconSansPadding from './ListItemIconSansPadding';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    button: {
      marginLeft: theme.spacing(1),
    },
    alert: {
      padding: theme.spacing(2),
    },
    listItem: {
      marginRight: theme.spacing(0.5),
    },
    bikeId: {
      marginRight: theme.spacing(1),
    },
  }),
);

export type BikeAction = {
  onClick: () => void;
  type: 'primary' | 'secondary' | 'default';
  label: string;
  id: string;
};

export type BikeActionsForBike = (bike: Bike) => BikeAction[];

type Props = {
  bikes: Bike[];
  bikeActions: BikeActionsForBike;
  showStatus: boolean;
  showLocation: boolean;
};

const BikesList = ({ bikes, bikeActions, showStatus, showLocation }: Props) => {
  const classes = useStyles();
  return (
    <>
      {!bikes.length && (
        <Typography variant="h5" className={classes.alert}>
          No bikes here!
        </Typography>
      )}
      {!!bikes.length && (
        <List dense={true}>
          {bikes.map(bike => (
            <ListItem key={bike.id} id={`bike-${bike.id}`}>
              <ListItemIconSansPadding>
                <DirectionsBikeIcon />
              </ListItemIconSansPadding>
              <Typography
                variant="h6"
                className={classes.bikeId}
              >{`#${bike.id}`}</Typography>

              {showStatus && (
                <Chip
                  label={`${bike.status}`}
                  className={classes.listItem}
                  variant={
                    bike.status == BikeStatus.Blocked ? 'outlined' : 'default'
                  }
                  color={
                    bike.status == BikeStatus.Available
                      ? 'secondary'
                      : bike.status == BikeStatus.Rented
                      ? 'primary'
                      : 'default'
                  }
                ></Chip>
              )}
              {showLocation && bike.user && (
                <Chip
                  label={`User: ${bike.user.name}`}
                  color="primary"
                  variant="outlined"
                  className={classes.listItem}
                />
              )}
              {showLocation && bike.station && (
                <Chip
                  label={`Station: ${bike.station.name}`}
                  color="secondary"
                  className={classes.listItem}
                />
              )}
              <ListItemSecondaryAction>
                {bikeActions(bike).map(({ onClick, label, type, id }) => (
                  <Button
                    variant="contained"
                    color={type}
                    onClick={onClick}
                    key={label}
                    className={classes.button}
                    id={id}
                  >
                    {label}
                  </Button>
                ))}
              </ListItemSecondaryAction>
            </ListItem>
          ))}
        </List>
      )}
    </>
  );
};

export default BikesList;
