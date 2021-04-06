import React from 'react';
import { DataSource } from '../hooks/usePromise';
import Alert from './Alert';
import Spinner from './Spinner';

type Props<T> = {
  data: DataSource<T>;
  children: (results: T) => React.ReactNode;
};

const DataLoader = <T,>({
  data: { loading, error, results },
  children,
}: Props<T>) => {
  return (
    <>
      {loading && <Spinner />}
      {error && <Alert severity="error">Oops! You are offline...</Alert>}
      {results && children(results)}
    </>
  );
};
export default DataLoader;
