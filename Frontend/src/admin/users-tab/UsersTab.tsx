import React from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import usePromise from '../../common/hooks/usePromise';
import useRefresh from '../../common/hooks/useRefresh';
import { useSnackbar } from '../../common/hooks/useSnackbar';
import SnackBar from '../../common/components/SnackBar';
import {
  Button,
  List,
  ListItem,
  ListItemSecondaryAction,
  ListItemText,
  Typography,
} from '@material-ui/core';
import ListItemIconSansPadding from '../../common/components/ListItemIconSansPadding';
import PeopleIcon from '@material-ui/icons/People';

const UsersTab = () => {
  const [refreshState] = useRefresh();
  const data = usePromise(useServices().getUsers, [refreshState]);
  const snackbar = useSnackbar();

  return (
    <>
      <DataLoader data={data}>
        {users =>
          !!users.length && (
            <List dense={true}>
              {users.map(user => (
                <ListItem key={user.id}>
                  <ListItemIconSansPadding>
                    <PeopleIcon />
                  </ListItemIconSansPadding>
                  <ListItemText
                    primary={
                      <Typography variant="h6">{`#${user.id} - ${user.name}`}</Typography>
                    }
                  />
                  <ListItemSecondaryAction>
                    {[].map(({ onClick, label, type }) => (
                      <Button
                        variant="contained"
                        color={type}
                        onClick={onClick}
                        key={label}
                        className={''}
                      >
                        {label}
                      </Button>
                    ))}
                  </ListItemSecondaryAction>
                </ListItem>
              ))}
            </List>
          )
        }
      </DataLoader>
      <SnackBar {...snackbar.props} />
    </>
  );
};

export default UsersTab;
