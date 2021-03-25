import React from 'react';
import './UserApp.css';
import {
  AppBar,
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

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      flexGrow: 1,
    },
    menuButton: {
      marginRight: theme.spacing(2),
    },
    title: {
      flexGrow: 1,
    },
  }),
);

const ApplicationBar = (): JSX.Element => {
  const classes = useStyles();
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <AppBar position="static">
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
            <MenuItem onClick={handleClose}>Profile</MenuItem>
            <MenuItem onClick={handleClose}>My account</MenuItem>
          </Menu>
        </div>
      </Toolbar>
    </AppBar>
  );
};

export default ApplicationBar;
