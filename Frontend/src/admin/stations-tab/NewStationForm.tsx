import React, { FormEvent, useState } from 'react';
import {
  Button,
  createStyles,
  makeStyles,
  TextField,
  Theme,
} from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    topBar: {
      paddingBottom: theme.spacing(2),
      display: 'flex',
      alignItems: 'flex-end',
    },
    input: {
      marginRight: theme.spacing(1),
    },
  }),
);

type Props = {
  onAdd: (name: string, bikesLimit?: number) => void;
};

const NewStationForm = ({ onAdd }: Props) => {
  const classes = useStyles();
  const [name, setName] = useState('');
  const [bikesLimit, setBikesLimit] = useState<number | ''>('');

  const submitHandler = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (name) {
      if (bikesLimit) {
        onAdd(name, bikesLimit);
      } else {
        onAdd(name);
      }
    }
    setName('');
  };

  return (
    <form className={classes.topBar} onSubmit={submitHandler}>
      <TextField
        id="new-station-name-input"
        label="New station's name"
        className={classes.input}
        value={name}
        onChange={e => setName(e.target.value)}
      />
      <TextField
        id="new-station-bikes-limit-input"
        label="New station's bikes limit"
        placeholder="10"
        className={classes.input}
        value={bikesLimit}
        onChange={e => setBikesLimit(parseInt(e.target.value) || '')}
      />
      <Button
        id="new-station-submit-button"
        variant="contained"
        color={'secondary'}
        type={'submit'}
      >
        Add station
      </Button>
    </form>
  );
};

export default NewStationForm;
