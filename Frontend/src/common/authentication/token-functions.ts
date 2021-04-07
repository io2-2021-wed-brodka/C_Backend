import { BearerToken } from './../api/models/bearer-token';
import { signIn } from './../api/endpoints';

const localStorageKey = 'token';

export const saveTokenInLocalStorage = (token: BearerToken) => {
  localStorage.setItem(localStorageKey, JSON.stringify(token));
};

export const clearTokenFromLocalStorage = () => {
  localStorage.removeItem(localStorageKey);
};

export const getTokenFromLocalStorage = () => {
  const tokenInJSON = localStorage.getItem(localStorageKey);

  if (tokenInJSON) {
    return JSON.parse(tokenInJSON) as BearerToken;
  }
  return null;
};

export const signInAndSaveToken = (login: string, password: string) => {
  return signIn(login, password).then(bearerToken => {
    saveTokenInLocalStorage(bearerToken);
    return bearerToken;
  });
};