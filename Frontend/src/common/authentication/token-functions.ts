import { BearerToken } from './../api/models/bearer-token';
import { signIn, signUp } from './../api/endpoints';

const localStorageTokenKey = 'token';
const localStorageLoginKey = 'login';

export const saveTokenAndLoginInLocalStorage = (
  token: BearerToken,
  login: string,
) => {
  localStorage.setItem(localStorageTokenKey, JSON.stringify(token));
  localStorage.setItem(localStorageLoginKey, login);
};

export const clearTokenAndLoginFromLocalStorage = () => {
  localStorage.removeItem(localStorageTokenKey);
  localStorage.removeItem(localStorageLoginKey);
};

export const getTokenFromLocalStorage = () => {
  const tokenInJSON = localStorage.getItem(localStorageTokenKey);

  if (tokenInJSON) {
    return JSON.parse(tokenInJSON) as BearerToken;
  }
  return null;
};

export const getLoginFromLocalStorage = () =>
  new Promise(resolve => {
    resolve(localStorage.getItem(localStorageLoginKey) || '');
  }) as Promise<string>;

export const signInAndSaveToken = (login: string, password: string) => {
  return signIn(login, password).then(user => {
    saveTokenAndLoginInLocalStorage({ token: user.token }, login);
    return user;
  });
};

export const signUpAndSaveToken = (login: string, password: string) => {
  return signUp(login, password).then(bearerToken => {
    saveTokenAndLoginInLocalStorage(bearerToken, login);
    return bearerToken;
  });
};
