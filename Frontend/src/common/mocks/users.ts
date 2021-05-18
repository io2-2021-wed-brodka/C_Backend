import { User, UserStatus } from './../api/models/user';

export const mockedUsers: User[] = [
  {
    id: '1',
    name: 'Janusz',
    status: UserStatus.Active,
  },
  {
    id: '2',
    name: 'Mietek',
    status: UserStatus.Active,
  },
  {
    id: '3',
    name: 'Zdzichu',
    status: UserStatus.Blocked,
  },
];
