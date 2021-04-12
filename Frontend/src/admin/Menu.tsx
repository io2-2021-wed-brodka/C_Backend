import React from 'react';
import List from '@material-ui/core/List';
import { ListItem, ListItemIcon, ListItemText } from '@material-ui/core';
import PeopleIcon from '@material-ui/icons/People';
import RoomIcon from '@material-ui/icons/Room';
import DirectionsBikeIcon from '@material-ui/icons/DirectionsBike';
import BuildIcon from '@material-ui/icons/Build';
import { Link } from 'react-router-dom';

const Menu = () => (
  <List>
    <ListItem button component={Link} to={'/stations'}>
      <ListItemIcon>
        <RoomIcon />
      </ListItemIcon>
      <ListItemText primary="Stations" />
    </ListItem>

    <ListItem button component={Link} to={'/bikes'}>
      <ListItemIcon>
        <DirectionsBikeIcon />
      </ListItemIcon>
      <ListItemText primary="Bikes" />
    </ListItem>

    <ListItem button component={Link} to={'/users'}>
      <ListItemIcon>
        <PeopleIcon />
      </ListItemIcon>
      <ListItemText primary="Users" />
    </ListItem>

    <ListItem button component={Link} to={'/techs'}>
      <ListItemIcon>
        <BuildIcon />
      </ListItemIcon>
      <ListItemText primary="Techs" />
    </ListItem>
  </List>
);

export default Menu;
