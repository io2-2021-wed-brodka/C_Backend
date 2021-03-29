import React, { useState } from 'react';
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  createStyles,
  makeStyles,
  Theme,
  Typography,
} from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import StationBikesList from './StationBikesList';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    heading: {
      fontSize: theme.typography.pxToRem(15),
      fontWeight: theme.typography.fontWeightRegular,
    },
  }),
);

type Props = {
  name: string;
  id: string;
};

const Station = ({ name, id }: Props) => {
  const classes = useStyles();
  const [hasBeenOpened, setHasBeenOpened] = useState(false);

  const handleChange = (_: unknown, isExpanded: boolean) => {
    if (isExpanded) setHasBeenOpened(true);
  };

  return (
    <>
      <Accordion onChange={handleChange}>
        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
          <Typography className={classes.heading}>{name}</Typography>
        </AccordionSummary>
        <AccordionDetails>
          {hasBeenOpened && (
            <StationBikesList stationId={id}></StationBikesList>
          )}
        </AccordionDetails>
      </Accordion>
    </>
  );
};

export default Station;
