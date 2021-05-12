export enum UserRole {
  User = 'user',
  Tech = 'tech',
  Admin = 'admin',
}

export interface User {
  token: string;
  role: UserRole;
}
