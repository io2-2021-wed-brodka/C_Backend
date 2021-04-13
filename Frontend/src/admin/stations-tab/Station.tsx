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
};

const Station = ({ name, id }: Props) => {
  const classes = useStyles();
  const [refreshState, refresh] = useRefresh();
  const snackbar = useSnackbar();
  const [hasBeenOpened, setHasBeenOpened] = useState(false);
  const addBike = useServices().addBike;
  const removeStation = useServices().removeStation;
  const blockStation = useServices().blockStation;

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
      .then(() => refresh())
      .catch(err => snackbar.open(err.message));
  };

  const onBlockStation = () => {
    blockStation(id)
      .then(() => refresh())
      .catch(err => snackbar.open(err.message));
  };

  return (
    <Accordion onChange={handleChange}>
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
              >
                Add bike
              </Button>{' '}
              <Button
                variant="contained"
                color={'default'}
                onClick={onBlockStation}
              >
                Block
              </Button>{' '}
              <Button
                variant="contained"
                color={'primary'}
                onClick={onRemoveStation}
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
