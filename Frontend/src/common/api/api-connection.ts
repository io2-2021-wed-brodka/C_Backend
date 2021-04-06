import axios, { AxiosRequestConfig } from 'axios';
const apiConnection = <T>(
  url: string,
  params?: AxiosRequestConfig,
): Promise<T> => {
  return axios({ ...params, url }).then(
    response => response.data as T,
  ) as Promise<T>;
};

export default apiConnection;
