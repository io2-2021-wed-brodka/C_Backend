import React from 'react';
import { useServices } from './../../common/services';
import DataLoader from '../../common/components/DataLoader';
import {
  Button,
  Dialog,
  DialogActions,
  DialogTitle,
  List,
  ListItem,
  ListItemText,
} from '@material-ui/core';

import usePromise from '../../common/hooks/usePromise';
import { Station } from './../../common/api/models/station';
import RoomIcon from '@material-ui/icons/Room';
import ListItemIconSansPadding from './../../common/components/ListItemIconSansPadding';

type Props = {
  close: () => void;
  selectStation: (station: Station) => void;
};

const StationsDialog = ({ close, selectStation }: Props) => {
  const stationsData = usePromise(useServices().getActiveStations);

  return (
    <div>
      <Dialog open onClose={close} scroll="paper">
        <DialogTitle>Select station</DialogTitle>
        <DataLoader data={stationsData}>
          {stations => (
            <List>
              {stations.map(station => (
                <ListItem
                  button
                  id={`return-on-station-${station.id}`}
                  key={station.name}
                  onClick={() => selectStation(station)}
                >
                  <>
                    <ListItemIconSansPadding>
                      <RoomIcon />
                    </ListItemIconSansPadding>
                    <ListItemText primary={station.name} />
                  </>
                </ListItem>
              ))}
            </List>
          )}
        </DataLoader>
        <DialogActions>
          <Button onClick={close}>Cancel</Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default StationsDialog;
