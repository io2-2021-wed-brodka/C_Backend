import { AxiosRequestConfig } from 'axios';
import apiConnection from '../api/api-connection';
import { getTokenFromLocalStorage } from './token-functions';

export const apiWithAuthConnection = <T>(
  url: string,
  params?: AxiosRequestConfig,
): Promise<T> => {
  const bearerToken = getTokenFromLocalStorage();
  if (bearerToken === null) {
    return Promise.reject(new Error('No token saved locally. Please sign in.'));
  }
  return apiConnection(url, {
    headers: { Authorization: `Bearer ${bearerToken}` },
    ...params,
  });
};
