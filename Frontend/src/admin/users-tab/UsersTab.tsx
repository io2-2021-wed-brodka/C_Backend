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
import { Button } from '@material-ui/core';

const UsersTab = () => {
  const [refreshState, refresh] = useRefresh();
  const getUsers = useServices().getUsers;
  const getBlockedUsers = useServices().getBlockedUsers;
  const data = usePromise(() => Promise.all([getUsers(), getBlockedUsers()]), [
    refreshState,
  ]);
  const snackbar = useSnackbar();
  const blockUser = useServices().blockUser;
  const unblockUser = useServices().unblockUser;

  const onBlockUser = (id: string) => {
    blockUser(id)
      .then(() => refresh())
      .catch(err => snackbar.open(err.message));
  };

  const onUnblockUser = (id: string) => {
    unblockUser(id)
      .then(() => refresh())
      .catch(err => snackbar.open(err.message));
  };

  return (
    <>
      <Typography variant="h4">Users</Typography>

      <Paper>
        <DataLoader data={data}>
          {([users, blockedUsers]) =>
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
                      {blockedUsers.some(
                        blockedUser => blockedUser.name === user.name,
                      ) ? (
                        <Button
                          variant="contained"
                          color={'default'}
                          onClick={() => onUnblockUser(user.id)}
                          key="Unblock"
                        >
                          Unblock
                        </Button>
                      ) : (
                        <Button
                          variant="contained"
                          color={'secondary'}
                          onClick={() => onBlockUser(user.id)}
                          key="Block"
                        >
                          Block
                        </Button>
                      )}
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
