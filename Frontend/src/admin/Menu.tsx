import React from 'react';
import List from '@material-ui/core/List';
import { ListItem, ListItemIcon, ListItemText } from '@material-ui/core';
import PeopleIcon from '@material-ui/icons/People';
import RoomIcon from '@material-ui/icons/Room';
import DirectionsBikeIcon from '@material-ui/icons/DirectionsBike';
import BuildIcon from '@material-ui/icons/Build';
import ArrowBackIcon from '@material-ui/icons/ArrowBack';
import { Link, useHistory } from 'react-router-dom';
import { clearTokenFromLocalStorage } from '../common/authentication/token-functions';

const Menu = () => {
  const history = useHistory();

  const logout = () => {
    clearTokenFromLocalStorage();
    history.push('/');
  };

  return (
    <List>
      <ListItem button component={Link} to={'/stations'} id="go-to-stations">
        <ListItemIcon>
          <RoomIcon />
        </ListItemIcon>
        <ListItemText primary="Stations" />
      </ListItem>

      <ListItem button component={Link} to={'/bikes'} id="go-to-bikes">
        <ListItemIcon>
          <DirectionsBikeIcon />
        </ListItemIcon>
        <ListItemText primary="Bikes" />
      </ListItem>

      <ListItem button component={Link} to={'/users'} id="go-to-users">
        <ListItemIcon>
          <PeopleIcon />
        </ListItemIcon>
        <ListItemText primary="Users" />
      </ListItem>

      <ListItem button component={Link} to={'/techs'} id="go-to-techs">
        <ListItemIcon>
          <BuildIcon />
        </ListItemIcon>
        <ListItemText primary="Techs" />
      </ListItem>

      <ListItem button onClick={logout} id="logout">
        <ListItemIcon>
          <ArrowBackIcon />
        </ListItemIcon>
        <ListItemText primary="Logout" />
      </ListItem>
    </List>
  );
};

export default Menu;
