import React from 'react';
import { Snackbar as MatSnackbar, IconButton } from '@material-ui/core';
import CloseIcon from '@material-ui/icons/Close';

type Props = {
  isOpen: boolean;
  setOpen: (newValue: boolean) => void;
  message: string;
};

export const SnackBar = ({ isOpen, setOpen, message }: Props) => {
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
          <IconButton
            size="small"
            aria-label="close"
            color="inherit"
            onClick={handleClose}
          >
            <CloseIcon fontSize="small" />
          </IconButton>
        }
      />
    </div>
  );
};

export default SnackBar;
