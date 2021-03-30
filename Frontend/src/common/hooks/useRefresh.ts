import { useReducer } from 'react';

const useRefresh = () => {
  const [, refresh] = useReducer(x => x + 1, 0);

  return refresh;
};

export default useRefresh;
