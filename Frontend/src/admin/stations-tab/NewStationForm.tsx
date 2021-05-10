import React, { FormEvent, useState } from 'react';
import { Button, createStyles, makeStyles, TextField, Theme } from '@material-ui/core';

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
  onAdd: (name: string) => void;
};

const NewStationForm = ({ onAdd }: Props) => {
  const classes = useStyles();
  const [name, setName] = useState('');

  const submitHandler = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (name) {
      onAdd(name);
    }
    setName('');
  };

  return (
    <form className={classes.topBar} onSubmit={submitHandler}>
      <TextField
        label="New station's name"
        className={classes.input}
        value={name}
        onChange={e => setName(e.target.value)}
      />
      <Button variant="contained" color={'secondary'} type={'submit'}>
        Add station
      </Button>
    </form>
  );
};

export default NewStationForm;
