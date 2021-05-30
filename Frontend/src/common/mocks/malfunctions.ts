import { Malfunction } from '../api/models/malfunction';

export const mockedMalfunctions: Malfunction[] = [
  {
    id: '1',
    bikeId: '33',
    reportingUserId: '2',
    description: 'This bike is very broken',
  },
  {
    id: '2',
    bikeId: '537',
    reportingUserId: '3',
    description: 'Wheels are missing',
  },
  {
    id: '3',
    bikeId: '3',
    reportingUserId: '4',
    description: 'I can only ride this bike backwards',
  },
];
