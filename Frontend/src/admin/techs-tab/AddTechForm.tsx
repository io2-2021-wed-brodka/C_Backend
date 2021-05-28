import {
  Button,
  createStyles,
  makeStyles,
  TextField,
  Theme,
} from '@material-ui/core';
import React, { FormEvent, useState } from 'react';

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
  onAdd: (name: string, password: string) => void;
};

const AddTechForm = ({ onAdd }: Props) => {
  const classes = useStyles();
  const [name, setName] = useState('');
  const [password, setPassword] = useState('');

  const submitHandler = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!name || !password) {
      return;
    }

    onAdd(name, password);
    setPassword('');
    setName('');
  };

  return (
    <form className={classes.topBar} onSubmit={submitHandler}>
      <TextField
        id="new-tech-name-input"
        label="New tech's name"
        className={classes.input}
        value={name}
        onChange={e => setName(e.target.value)}
      />
      <TextField
        id="new-tech-password-input"
        label="New tech's password"
        className={classes.input}
        value={password}
        type="password"
        onChange={e => setPassword(e.target.value)}
      />
      <Button
        id="new-tech-submit-button"
        variant="contained"
        color={'secondary'}
        type={'submit'}
        disabled={!name || !password}
      >
        Add tech
      </Button>
    </form>
  );
};

export default AddTechForm;
