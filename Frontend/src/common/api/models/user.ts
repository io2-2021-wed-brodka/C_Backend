export enum UserStatus {
  Active = 'active',
  Blocked = 'blocked',
}

export interface User {
  id: string;
  name: string;
  status: UserStatus;
}
