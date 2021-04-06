import React from 'react';
import {
  createStyles,
  Button,
  List,
  ListItem,
  ListItemIcon,
  ListItemSecondaryAction,
  ListItemText,
  makeStyles,
  Theme,
  Typography,
} from '@material-ui/core';
import DirectionsBikeIcon from '@material-ui/icons/DirectionsBike';
import { Bike } from '../api/models/bike';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    button: {
      marginLeft: theme.spacing(1),
    },
    alert: {
      marginLeft: theme.spacing(2),
    },
    icon: {
      minWidth: 36,
    },
  }),
);

export type BikeAction = {
  onClick: () => void;
  type: 'primary' | 'secondary' | 'default';
  label: string;
};

export type BikeActionsForBike = (bikeId: string) => BikeAction[];

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
      <List dense={true}>
        {bikes.map(({ id }) => (
          <ListItem key={id}>
            <ListItemIcon className={classes.icon}>
              <DirectionsBikeIcon />
            </ListItemIcon>
            <ListItemText
              primary={<Typography variant="h6">{`#${id}`}</Typography>}
            />
            <ListItemSecondaryAction>
              {bikeActions(id).map(({ onClick, label, type }) => (
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
    </>
  );
};

export default BikesList;
