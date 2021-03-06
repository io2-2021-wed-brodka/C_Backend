import { useEffect, useState } from 'react';
import { DataSource } from './usePromise';

const useMockedApi = <T>(
  expectedResult?: T,
  expectedError?: string,
): DataSource<T> => {
  const [loading, setLoading] = useState(true);
  const [results, setResults] = useState<T | null>(null);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    setTimeout(() => {
      setLoading(false);
      if (expectedError) {
        setError(new Error(expectedError));
      } else if (expectedResult) {
        setResults(expectedResult);
      }
    }, Math.random() * 500 + 500);
  }, []);

  return {
    error,
    loading,
    results,
  } as const;
};

export default useMockedApi;
