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
};

export type BikeActionsForBike = (bike: Bike) => BikeAction[];

type Props = {
  bikes: Bike[];
  bikeActions: BikeActionsForBike;
};

const BikesList = ({ bikes, bikeActions }: Props) => {
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
            <ListItem key={bike.id}>
              <ListItemIconSansPadding>
                <DirectionsBikeIcon />
              </ListItemIconSansPadding>
              <ListItemText
                primary={<Typography variant="h6">{`#${bike.id}`}</Typography>}
              />
              <ListItemSecondaryAction>
                {bikeActions(bike).map(({ onClick, label, type }) => (
                  <Button
                    variant="contained"
                    color={type}
                    onClick={onClick}
                    key={label}
                    className={classes.button}
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
