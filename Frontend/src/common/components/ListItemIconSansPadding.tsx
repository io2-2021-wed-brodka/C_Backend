import React from 'react';
import { createStyles, ListItemIcon, makeStyles } from '@material-ui/core';

const useStyles = makeStyles(() =>
  createStyles({
    icon: {
      minWidth: 36,
    },
  }),
);

type Props = {
  children: React.ReactNode;
};

const ListItemIconSansPadding = ({ children }: Props) => {
  const classes = useStyles();
  return <ListItemIcon className={classes.icon}>{children}</ListItemIcon>;
};

export default ListItemIconSansPadding;
