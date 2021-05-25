import React from 'react';
import {
  createStyles,
  Button,
  List,
  ListItem,
  ListItemSecondaryAction,
  ListItemText,
  makeStyles,
  Theme,
  Typography,
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
              <ListItemText
                primary={<Typography variant="h6">{`#${bike.id}`}</Typography>}
              />
              {showStatus && (
                <ListItemText
                  primary={
                    <Typography variant="subtitle1">{`Status: ${bike.status}`}</Typography>
                  }
                />
              )}
              {showLocation && (
                <ListItemText
                  primary={
                    bike.user != null ? (
                      <Typography variant="subtitle1">{`At user: ${bike.user.name}`}</Typography>
                    ) : bike.station != null ? (
                      <Typography variant="subtitle1">{`At station: ${bike.station.name}`}</Typography>
                    ) : (
                      <Typography variant="subtitle1">{`Unknown location`}</Typography>
                    )
                  }
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
