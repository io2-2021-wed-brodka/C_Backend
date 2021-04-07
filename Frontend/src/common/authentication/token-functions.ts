import { BearerToken } from './../api/models/bearer-token';
import { signIn } from './../api/endpoints';

export const saveTokenInLocalStorage = (token: BearerToken) => {
  localStorage.setItem('token', JSON.stringify(token));
};

export const getTokenFromLocalStorage = () => {
  const tokenInJSON = localStorage.getItem('token');

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
