import React from 'react';
import { mockedServices, useServices } from '../../common/services';
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

const TechsTab = () => {
  const [refreshState, refresh] = useRefresh();
  //const getTechs = useServices().getUsers;
  const getTechs = mockedServices.getTechs;
  const data = usePromise(() => Promise.all([getTechs()]), [refreshState]);
  const snackbar = useSnackbar();

  return (
    <>
      <Typography variant="h4">Techs</Typography>
      <Paper>
        <DataLoader data={data}>
          {([techs, blockedUsers]) =>
            !!techs.length && (
              <List dense={true}>
                {techs.map(tech => (
                  <ListItem key={tech.id}>
                    <ListItemIconSansPadding>
                      <PersonIcon />
                    </ListItemIconSansPadding>
                    <ListItemText
                      primary={
                        <Typography variant="h6">{`${tech.name}`}</Typography>
                      }
                    />
                    <ListItemSecondaryAction></ListItemSecondaryAction>
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

export default TechsTab;
