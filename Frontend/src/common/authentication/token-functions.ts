import { signIn, signUp } from './../api/endpoints';
import { UserRole } from '../api/models/login-response';
import { isUserApp } from '../environment';

const prefix = isUserApp() ? 'user_' : 'admin_';

const localStorageTokenKey = prefix + 'token';
const localStorageLoginKey = prefix + 'login';
const localStorageRoleKey = prefix + 'role';

export const saveUserDataInLocalStorage = (
  token: string,
  login: string,
  role: UserRole,
) => {
  localStorage.setItem(localStorageTokenKey, token);
  localStorage.setItem(localStorageLoginKey, login);
  localStorage.setItem(localStorageRoleKey, role);
};

export const removeUserDataFromLocalStorage = () => {
  localStorage.removeItem(localStorageTokenKey);
  localStorage.removeItem(localStorageLoginKey);
  localStorage.removeItem(localStorageRoleKey);
};

export const getTokenFromLocalStorage = () => {
  const token = localStorage.getItem(localStorageTokenKey);
  return token;
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
