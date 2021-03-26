import React from 'react';
import { createStyles, makeStyles, Paper, Theme } from '@material-ui/core';
import DirectionsBikeIcon from '@material-ui/icons/DirectionsBike';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    paper: {
      height: 100,
      width: 100,
      cursor: 'pointer',
      background: theme.palette.secondary.light,
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'space-around',
      color: theme.palette.text.secondary,
      fontSize: theme.typography.pxToRem(28),
      fontFamily: theme.typography.fontFamily,
    },
  }),
);

type BikeTileProps = {
  id: string;
};

const BikeTile = ({ id }: BikeTileProps): JSX.Element => {
  const classes = useStyles();
  return (
    <Paper className={classes.paper}>
      <DirectionsBikeIcon style={{ fontSize: 30 }}></DirectionsBikeIcon>
      {`#${id}`}
    </Paper>
  );
};

export default BikeTile;
