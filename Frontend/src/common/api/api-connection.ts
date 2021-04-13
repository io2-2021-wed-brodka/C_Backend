import axios, { AxiosRequestConfig } from 'axios';
const apiConnection = <T>(
  url: string,
  params?: AxiosRequestConfig,
): Promise<T> => {
  return axios({ ...params, url })
    .then(response => response.data as T)
    .catch(err => {
      throw new Error(err?.response?.data?.message || err.message);
    }) as Promise<T>;
};

export default apiConnection;
