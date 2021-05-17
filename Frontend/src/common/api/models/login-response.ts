export enum UserRole {
  User = 'user',
  Tech = 'tech',
  Admin = 'admin',
}

export interface LoginResponse {
  token: string;
  role: UserRole;
}
