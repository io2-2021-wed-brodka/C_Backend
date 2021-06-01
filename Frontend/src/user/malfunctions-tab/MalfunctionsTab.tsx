import React from 'react';
import { useServices } from '../../common/services';
import DataLoader from '../../common/components/DataLoader';
import usePromise from '../../common/hooks/usePromise';
import useRefresh from '../../common/hooks/useRefresh';
import { useSnackbar } from '../../common/hooks/useSnackbar';
import SnackBar from '../../common/components/SnackBar';
import {
  Button,
  Paper,
  Typography,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  makeStyles,
  createStyles,
  Theme,
} from '@material-ui/core';
import ListItemIconSansPadding from '../../common/components/ListItemIconSansPadding';
import DirectionsBikeIcon from '@material-ui/icons/DirectionsBike';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    inline: {
      display: 'inline',
    },
    bikeId: {
      marginRight: theme.spacing(1),
    },
    paddingRight: {
      paddingRight: '5em',
    },
  }),
);

const MalfunctionsTab = () => {
  const [refreshState, refresh] = useRefresh();
  const getMalfunctions = useServices().getMalfunctions;
  const removeMalfunction = useServices().removeMalfunction;
  const data = usePromise(getMalfunctions, [refreshState]);
  const snackbar = useSnackbar();
  const classes = useStyles();

  const onRemoveMalfunction = (id: string) => {
    removeMalfunction(id)
      .then(() => refresh())
      .catch(err => snackbar.open(err.message));
  };

  return (
    <>
      <Typography variant="h4" id="malfunctions-header">
        Malfunctions
      </Typography>

      <Paper>
        <DataLoader data={data}>
          {malfunctions =>
            malfunctions.length ? (
              <List>
                {malfunctions.map(({ id, bikeId, description }) => (
                  <ListItem key={id}>
                    <ListItemIconSansPadding>
                      <DirectionsBikeIcon />
                    </ListItemIconSansPadding>
                    <Typography
                      variant="h6"
                      className={classes.bikeId}
                    >{`#${bikeId}`}</Typography>
                    <ListItemText
                      secondary={description}
                      className={classes.paddingRight}
                    />
                    <ListItemSecondaryAction>
                      <Button
                        variant="contained"
                        color={'primary'}
                        onClick={() => onRemoveMalfunction(id)}
                        id={`remove-${id}`}
                      >
                        Remove
                      </Button>
                    </ListItemSecondaryAction>
                  </ListItem>
                ))}
              </List>
            ) : (
              'No malfunctions'
            )
          }
        </DataLoader>
        <SnackBar {...snackbar.props} />
      </Paper>
    </>
  );
};

export default MalfunctionsTab;
