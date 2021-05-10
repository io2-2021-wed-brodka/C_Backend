import React from 'react';
import './UserApp.css';
import { createStyles, IconButton, makeStyles, Menu, MenuItem, Theme, Toolbar, Typography } from '@material-ui/core';
import DirectionsBikeIcon from '@material-ui/icons/DirectionsBike';
import AccountCircle from '@material-ui/icons/AccountCircle';
import { clearTokenFromLocalStorage } from './../common/authentication/token-functions';
import { useHistory } from 'react-router-dom';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    menuButton: {
      marginRight: theme.spacing(2),
    },
    title: {
      flexGrow: 1,
    },
  }),
);

const ApplicationBar = () => {
  const classes = useStyles();
  const history = useHistory();
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const logout = () => {
    clearTokenFromLocalStorage();
    history.push('/');
  };

  return (
    <Toolbar>
      <IconButton edge="start" className={classes.menuButton} color="inherit" aria-label="menu">
        <DirectionsBikeIcon />
      </IconButton>
      <Typography variant="h6" className={classes.title}>
        CityBikes
      </Typography>
      <div>
        <IconButton onClick={handleMenu} color="inherit">
          <AccountCircle />
        </IconButton>
        <Menu id="menu-appbar" anchorEl={anchorEl} open={open} onClose={handleClose}>
          <MenuItem onClick={logout}>Logout</MenuItem>
        </Menu>
      </div>
    </Toolbar>
  );
};

export default ApplicationBar;
