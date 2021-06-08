import React from 'react';
import {
  Paper,
  Typography,
  makeStyles,
  createStyles,
  Theme,
} from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    paper: {
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      padding: theme.spacing(4),
    },
    h4: {
      color: theme.palette.secondary.dark,
    },
    paddingRight: {
      paddingRight: '5em',
    },
  }),
);

const ContactTab = () => {
  const classes = useStyles();

  return (
    <Paper className={classes.paper}>
      <Typography variant="h5">Please call us!</Typography>
      <Typography variant="h4" className={classes.h4}>
        +48 123 456 789
      </Typography>
    </Paper>
  );
};

export default ContactTab;
