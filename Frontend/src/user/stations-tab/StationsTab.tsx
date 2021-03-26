import React from 'react';
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
import Grid from '@material-ui/core/Grid';
import BikeTile from './BikeTile';
import useMockedDataFetching from './../../common/mocks/useMockedDataFetching';
import { mockedStations } from './../../common/mocks/stations';
import Alert from '../../common/components/Alert';
import Spinner from '../../common/components/Spinner';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    heading: {
      fontSize: theme.typography.pxToRem(15),
      fontWeight: theme.typography.fontWeightRegular,
    },
  }),
);

const useDataFetching = useMockedDataFetching(mockedStations);

const StationsTab = (): JSX.Element => {
  const classes = useStyles();
  const { results: stations, error, loading } = useDataFetching('/stations');

  return (
    <>
      {loading && <Spinner />}
      {error && <Alert severity="error">Oops! You are offline...</Alert>}
      {stations &&
        stations.map(({ name, id }) => (
          <Accordion key={id}>
            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
              <Typography className={classes.heading}>{name}</Typography>
            </AccordionSummary>
            <AccordionDetails>
              <Grid container justify="center" spacing={2}>
                {[0, 1, 2].map((value) => (
                  <Grid key={value} item>
                    <BikeTile id={value * 100} />
                  </Grid>
                ))}
              </Grid>
            </AccordionDetails>
          </Accordion>
        ))}
    </>
  );
};

export default StationsTab;
