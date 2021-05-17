import React from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import usePromise from '../../common/hooks/usePromise';
import useRefresh from '../../common/hooks/useRefresh';
import { useSnackbar } from '../../common/hooks/useSnackbar';
import SnackBar from '../../common/components/SnackBar';
import {
  List,
  ListItem,
  ListItemSecondaryAction,
  ListItemText,
  Paper,
  Typography,
} from '@material-ui/core';
import ListItemIconSansPadding from '../../common/components/ListItemIconSansPadding';
import PersonIcon from '@material-ui/icons/Person';

const UsersTab = () => {
  const [refreshState] = useRefresh();
  const data = usePromise(useServices().getUsers, [refreshState]);
  const snackbar = useSnackbar();

  return (
    <>
      <Typography variant="h4">Users</Typography>

      <Paper>
        <DataLoader data={data}>
          {users =>
            !!users.length && (
              <List dense={true}>
                {users.map(user => (
                  <ListItem key={user.id}>
                    <ListItemIconSansPadding>
                      <PersonIcon />
                    </ListItemIconSansPadding>
                    <ListItemText
                      primary={
                        <Typography variant="h6">{`${user.name}`}</Typography>
                      }
                    />
                    <ListItemSecondaryAction>
                      Buttons to block/unblock go here
                    </ListItemSecondaryAction>
                  </ListItem>
                ))}
              </List>
            )
          }
        </DataLoader>
        <SnackBar {...snackbar.props} />
      </Paper>
    </>
  );
};

export default UsersTab;
