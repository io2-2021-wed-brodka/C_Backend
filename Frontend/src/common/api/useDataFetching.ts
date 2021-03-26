import { useEffect, useState } from 'react';

type Data<T> = {
  error: string;
  loading: boolean;
  results: T;
};

const useDataFetching = <T>(dataSource: string): Data<T> => {
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

export default useDataFetching;
