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
  Chip,
} from '@material-ui/core';
import DirectionsBikeIcon from '@material-ui/icons/DirectionsBike';
import { Bike } from '../api/models/bike';
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
              <Typography variant="h6">{`#${bike.id}`}</Typography>

              {showStatus && (
                <Chip
                  label={`Status: ${bike.status}`}
                  variant="outlined"
                ></Chip>
              )}
              {showLocation &&
                (bike.user != null ? (
                  <Chip
                    label={`At user: ${bike.user.name}`}
                    variant="outlined"
                  />
                ) : bike.station != null ? (
                  <Chip
                    label={`At station: ${bike.station.name}`}
                    variant="outlined"
                  />
                ) : (
                  <Chip label={`Unknown location`} variant="outlined" />
                ))}

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
