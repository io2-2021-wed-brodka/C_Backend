import { useReducer } from 'react';

const useRefresh = () => {
  const [refreshState, refresh] = useReducer(x => x + 1, 0);

  return [refreshState, refresh];
};

export default useRefresh;
