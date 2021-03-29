import { useEffect, useState } from 'react';

export type DataSource<T> = {
  error: string;
  loading: boolean;
  results: T;
};

const useApi = <T>(dataSource: string): DataSource<T> => {
  const [loading, setLoading] = useState(true);
  const [results, setResults] = useState<T>();
  const [error, setError] = useState('');

  useEffect(() => {
    async function fetchData() {
      try {
        const data = await fetch(dataSource);
        const json = await data.json();

        if (json) {
          setLoading(false);
          setResults(json);
        }
      } catch (error) {
        setLoading(false);
        setError(error.message);
      }

      setLoading(false);
    }

    fetchData();
  }, [dataSource]);

  return {
    error,
    loading,
    results: results as T,
  } as const;
};

export default useApi;
