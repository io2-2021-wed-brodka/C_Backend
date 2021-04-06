import { useCallback, useState } from 'react';

export const useSnackbar = () => {
  const [isOpen, setOpen] = useState(false);
  const [message, setMessage] = useState('');

  const open = useCallback((message: string) => {
    setMessage(message);
    setOpen(true);
  }, []);

  return { open, snackbarProps: { isOpen, setOpen, message } };
};
