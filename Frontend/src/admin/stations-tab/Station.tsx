import React, { useState } from 'react';
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Button,
  createStyles,
  Divider,
  makeStyles,
  Theme,
  Typography,
} from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import StationBikesList from './StationBikesList';
import { useServices } from '../../common/services';
import useRefresh from '../../common/hooks/useRefresh';
import SnackBar from '../../common/components/SnackBar';
import { useSnackbar } from '../../common/hooks/useSnackbar';
import { StationStatus } from '../../common/api/models/station';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    heading: {
      fontSize: theme.typography.pxToRem(15),
      fontWeight: theme.typography.fontWeightRegular,
    },
    accordionDetails: {
      padding: 0,
    },
    stationButtonsDiv: {
      paddingLeft: theme.spacing(2),
      paddingBottom: theme.spacing(2),
    },
    div: {
      width: '100%',
    },
  }),
);

type Props = {
  name: string;
  id: string;
  status: StationStatus;
  forceRefresh?: () => void;
};

const Station = ({ name, id, status, forceRefresh }: Props) => {
  const classes = useStyles();
  const [refreshState, refresh] = useRefresh();
  const snackbar = useSnackbar();
  const [hasBeenOpened, setHasBeenOpened] = useState(false);
  const addBike = useServices().addBike;
  const removeStation = useServices().removeStation;
  const blockStation = useServices().blockStation;
  const unblockStation = useServices().unblockStation;

  const handleChange = (_: unknown, isExpanded: boolean) => {
    if (isExpanded) {
      setHasBeenOpened(true);
    }
  };

  const onAddBike = () => {
    addBike(id)
      .then(() => refresh())
      .catch(err => snackbar.open(err.message));
  };

  const onRemoveStation = () => {
    removeStation(id)
      .then(() => forceRefresh && forceRefresh())
      .catch(err => snackbar.open(err.message));
  };

  const onBlockStation = () => {
    blockStation(id)
      .then(() => forceRefresh && forceRefresh())
      .catch(err => snackbar.open(err.message));
  };

  const onUnblockStation = () => {
    unblockStation(id)
      .then(() => forceRefresh && forceRefresh())
      .catch(err => snackbar.open(err.message));
  };

  return (
    <Accordion onChange={handleChange} id={`station-${name}`}>
      <AccordionSummary expandIcon={<ExpandMoreIcon />}>
        <Typography className={classes.heading}>{name}</Typography>
      </AccordionSummary>
      <AccordionDetails className={classes.accordionDetails}>
        {hasBeenOpened && (
          <div className={classes.div}>
            <div className={classes.stationButtonsDiv}>
              <Button
                variant="contained"
                color={'secondary'}
                onClick={onAddBike}
                id="add-bike"
              >
                Add bike
              </Button>{' '}
              {status === StationStatus.Active && (
                <Button
                  variant="contained"
                  color={'default'}
                  onClick={onBlockStation}
                  id={`block-station-${name}`}
                >
                  Block
                </Button>
              )}
              {status === StationStatus.Blocked && (
                <Button
                  variant="contained"
                  color={'default'}
                  onClick={onUnblockStation}
                  id={`unblock-station-${name}`}
                >
                  Unblock
                </Button>
              )}{' '}
              <Button
                variant="contained"
                color={'primary'}
                onClick={onRemoveStation}
                id={`remove-station-${name}`}
              >
                Remove
              </Button>
            </div>
            <Divider />
            <StationBikesList
              stationId={id}
              refresh={refreshState}
            ></StationBikesList>
          </div>
        )}
      </AccordionDetails>
      <SnackBar {...snackbar.props} />
    </Accordion>
  );
};

export default Station;
