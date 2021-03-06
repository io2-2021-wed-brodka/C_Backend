import React, { useState } from 'react';
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Button,
  Chip,
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
    listItem: {
      marginRight: theme.spacing(0.5),
    },
  }),
);

type Props = {
  name: string;
  id: string;
  status: StationStatus;
  reservationsCount?: number;
  malfunctionsCount?: number;
  activeBikesCount: number;
  forceRefresh?: () => void;
};

const Station = ({
  name,
  id,
  status,
  forceRefresh,
  malfunctionsCount,
  reservationsCount,
  activeBikesCount,
}: Props) => {
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
        <Typography className={classes.listItem} variant="h6">
          {name}
        </Typography>
        <Chip
          id={`chip-reserved-${name}`}
          label={`Reserv.: ${reservationsCount}`}
          color="default"
          className={classes.listItem}
          data-count={reservationsCount}
        />
        <Chip
          id={`chip-malfunctions-${name}`}
          label={`Mal.: ${malfunctionsCount}`}
          color="primary"
          className={classes.listItem}
          data-count={malfunctionsCount}
        />
        <Chip
          id={`chip-active-${name}`}
          label={`Act. bikes: ${activeBikesCount}`}
          color="secondary"
          className={classes.listItem}
          data-count={activeBikesCount}
        />
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
