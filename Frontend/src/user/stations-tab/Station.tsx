import React, { useState } from 'react';
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Chip,
  createStyles,
  makeStyles,
  Theme,
  Typography,
} from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import StationBikesList from './StationBikesList';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    accordionDetails: {
      padding: 0,
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
  activeBikesCount: number;
};

const Station = ({ name, id, activeBikesCount }: Props) => {
  const classes = useStyles();
  const [hasBeenOpened, setHasBeenOpened] = useState(false);

  const handleChange = (_: unknown, isExpanded: boolean) => {
    if (isExpanded) {
      setHasBeenOpened(true);
    }
  };

  return (
    <Accordion onChange={handleChange} id={`station-${name}`}>
      <AccordionSummary expandIcon={<ExpandMoreIcon />}>
        <Typography className={classes.listItem} variant="h6">
          {name}
        </Typography>
        <Chip
          id={`chip-active-${name}`}
          label={activeBikesCount}
          color="secondary"
          className={classes.listItem}
          data-count={activeBikesCount}
        />
      </AccordionSummary>
      <AccordionDetails className={classes.accordionDetails}>
        {hasBeenOpened && (
          <div className={classes.div}>
            <StationBikesList stationId={id}></StationBikesList>
          </div>
        )}
      </AccordionDetails>
    </Accordion>
  );
};

export default Station;
