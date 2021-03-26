import { useEffect, useState } from 'react';

type Data<T> = {
  error: string;
  loading: boolean;
  results: T;
};

const useMockedDataFetching = <T>(
  expectedResult?: T,
  expectedError?: string,
): ((_: string) => Data<T>) => () => {
  const [loading, setLoading] = useState(true);
  const [results, setResults] = useState<T>();
  const [error, setError] = useState('');

  useEffect(() => {
    setTimeout(() => {
      setLoading(false);
      if (expectedError) {
        setError(expectedError);
      } else {
        setResults(expectedResult);
      }
    }, Math.random() * 500 + 500);
  }, []);

  return {
    error,
    loading,
    results: results as T,
  } as const;
};

export default useMockedDataFetching;
