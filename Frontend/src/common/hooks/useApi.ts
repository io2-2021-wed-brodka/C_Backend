import { useEffect, useState } from 'react';
import axios, { AxiosRequestConfig } from 'axios';

export type DataSource<T> = {
  error: string;
  loading: boolean;
  results: T | null;
};

const useApi = <T>(url: string, params?: AxiosRequestConfig): DataSource<T> => {
  const [loading, setLoading] = useState(true);
  const [results, setResults] = useState<T | null>(null);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const { data } = await axios({ ...params, url });

        if (data) {
          setResults(data);
        }
      } catch (error) {
        setError(error.data.message);
      }

      setLoading(false);
    };

    fetchData();
  }, [url]);

  return {
    error,
    loading,
    results,
  } as const;
};

export default useApi;
