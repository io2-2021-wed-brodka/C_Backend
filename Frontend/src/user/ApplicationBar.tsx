import React from 'react';
import './UserApp.css';
import {
  createStyles,
  IconButton,
  makeStyles,
  Menu,
  MenuItem,
  Theme,
  Toolbar,
  Typography,
} from '@material-ui/core';
import DirectionsBikeIcon from '@material-ui/icons/DirectionsBike';
import AccountCircle from '@material-ui/icons/AccountCircle';
import { removeUserDataFromLocalStorage } from './../common/authentication/token-functions';
import { useHistory } from 'react-router-dom';
import { useServices } from '../common/services';
import usePromise from '../common/hooks/usePromise';

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
  const getLogin = useServices().getLogin;
  const login = usePromise(getLogin);
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const logout = () => {
    removeUserDataFromLocalStorage();
    history.push('/');
  };

  return (
    <Toolbar>
      <IconButton
        edge="start"
        className={classes.menuButton}
        color="inherit"
        aria-label="menu"
      >
        <DirectionsBikeIcon />
      </IconButton>
      <Typography variant="h6" className={classes.title}>
        {login.results && `Hello, ${login.results}!`}
      </Typography>
      <div>
        <IconButton onClick={handleMenu} color="inherit" id="account-menu">
          <AccountCircle />
        </IconButton>
        <Menu
          id="menu-appbar"
          anchorEl={anchorEl}
          open={open}
          onClose={handleClose}
        >
          <MenuItem onClick={logout} id="logout">
            Logout
          </MenuItem>
        </Menu>
      </div>
    </Toolbar>
  );
};

export default ApplicationBar;
