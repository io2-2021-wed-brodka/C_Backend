import React from 'react';
import {
  CircularProgress,
  Container,
  createStyles,
  makeStyles,
  Theme,
} from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      display: 'flex',
      justifyContent: 'center',
    },
    circle: {
      color: theme.palette.secondary.main,
    },
  }),
);

const Spinner = (): JSX.Element => {
  const classes = useStyles();
  return (
    <Container className={classes.root}>
      <CircularProgress className={classes.circle} />
    </Container>
  );
};
export default Spinner;
