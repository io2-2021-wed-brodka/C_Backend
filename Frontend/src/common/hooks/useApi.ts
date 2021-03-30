import { useEffect, useState } from 'react';

export type DataSource<T> = {
  error: string;
  loading: boolean;
  results: T | null;
};

const useApi = <T>(url: string, params?: Request): DataSource<T> => {
  const [loading, setLoading] = useState(true);
  const [results, setResults] = useState<T | null>(null);
  const [error, setError] = useState('');

  useEffect(() => {
    async function fetchData() {
      try {
        const data = await fetch(params ? { ...params, url } : url);
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
  }, [url]);

  return {
    error,
    loading,
    results,
  } as const;
};

export default useApi;
