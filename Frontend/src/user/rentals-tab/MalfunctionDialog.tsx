import React, { useState } from 'react';
import {
  Button,
  createStyles,
  Dialog,
  DialogActions,
  DialogTitle,
  makeStyles,
  TextField,
  Theme,
} from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    margin: {
      margin: theme.spacing(1),
    },
  }),
);

type Props = {
  close: () => void;
  bikeId: string;
  onReportMalfunction: (description: string) => void;
};

const MalfunctionDialog = ({ bikeId, close, onReportMalfunction }: Props) => {
  const [description, setDescription] = useState('');
  const classes = useStyles();

  return (
    <div>
      <Dialog open onClose={close} scroll="paper">
        <DialogTitle>What is wrong with bike #{bikeId}?</DialogTitle>
        <TextField
          id="malfunction-description"
          label="Malfunction description"
          multiline
          rows={4}
          variant="outlined"
          className={classes.margin}
          onChange={e => setDescription(e.target.value)}
        />
        <DialogActions>
          <Button
            onClick={() => onReportMalfunction(description)}
            disabled={!description}
          >
            Report
          </Button>
          <Button onClick={close}>Cancel</Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default MalfunctionDialog;
