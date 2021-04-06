import { useEffect, useState } from 'react';

export type DataSource<T> = {
  error: Error | null;
  loading: boolean;
  results: T | null;
};

const usePromise = <T>(
  promise: () => Promise<T>,
  dependencies: unknown[] = [],
): DataSource<T> => {
  const [loading, setLoading] = useState(true);
  const [results, setResults] = useState<T | null>(null);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    promise()
      .then(results => setResults(results))
      .catch(err => setError(err as Error))
      .finally(() => setLoading(false));

    return () => {
      setLoading(true);
      setError(null);
      setResults(null);
    };
  }, dependencies);

  return {
    error,
    loading,
    results,
  } as const;
};

export default usePromise;
