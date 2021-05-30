import { BearerToken } from './../api/models/bearer-token';
import { signIn, signUp } from './../api/endpoints';
import { UserRole } from '../api/models/login-response';

const localStorageTokenKey = 'token';
const localStorageLoginKey = 'login';
const localStorageRoleKey = 'role';

export const saveUserDataInLocalStorage = (
  token: string,
  login: string,
  role: UserRole,
) => {
  localStorage.setItem(localStorageTokenKey, JSON.stringify(token));
  localStorage.setItem(localStorageLoginKey, login);
  localStorage.setItem(localStorageRoleKey, role);
};

export const removeUserDataFromLocalStorage = () => {
  localStorage.removeItem(localStorageTokenKey);
  localStorage.removeItem(localStorageLoginKey);
  localStorage.removeItem(localStorageRoleKey);
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

export const getRoleFromLocalStorage = () =>
  new Promise(resolve => {
    resolve(localStorage.getItem(localStorageRoleKey) as UserRole);
  }) as Promise<UserRole | null>;

export const signInAndSaveToken = (login: string, password: string) => {
  return signIn(login, password).then(user => {
    saveUserDataInLocalStorage(user.token, login, user.role);
    return user;
  });
};

export const signUpAndSaveToken = (login: string, password: string) => {
  return signUp(login, password).then(bearerToken => {
    saveUserDataInLocalStorage(bearerToken.token, login, UserRole.User);
    return bearerToken;
  });
};
