import React from 'react';
import { Snackbar as MatSnackbar, Button, IconButton } from '@material-ui/core';
import CloseIcon from '@material-ui/icons/Close';

type Props = {
  isOpen: boolean;
  setOpen: (newValue: boolean) => void;
  message: string;
};

export const Snackbar = ({ isOpen, setOpen, message }: Props) => {
  const handleClose = (
    event: React.SyntheticEvent | React.MouseEvent,
    reason?: string,
  ) => {
    if (reason === 'clickaway') {
      return;
    }

    setOpen(false);
  };

  return (
    <div>
      <MatSnackbar
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'center',
        }}
        open={isOpen}
        autoHideDuration={4000}
        onClose={handleClose}
        message={message}
        action={
          <React.Fragment>
            <Button color="secondary" size="small" onClick={handleClose}>
              CLOSE
            </Button>
            <IconButton
              size="small"
              aria-label="close"
              color="inherit"
              onClick={handleClose}
            >
              <CloseIcon fontSize="small" />
            </IconButton>
          </React.Fragment>
        }
      />
    </div>
  );
};

export default Snackbar;
