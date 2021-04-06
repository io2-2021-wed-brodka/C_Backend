import { useEffect, useState } from 'react';
import { AxiosRequestConfig } from 'axios';
import apiConnection from '../api/api-connection';

export type DataSource<T> = {
  error: Error | null;
  loading: boolean;
  results: T | null;
};

const useApi = <T>(url: string, params?: AxiosRequestConfig): DataSource<T> => {
  const [loading, setLoading] = useState(true);
  const [results, setResults] = useState<T | null>(null);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    apiConnection<T>(url, params)
      .then(results => setResults(results))
      .catch(err => setError(err as Error))
      .finally(() => setLoading(false));
  }, [url, params]);

  return {
    error,
    loading,
    results,
  } as const;
};

export default useApi;
