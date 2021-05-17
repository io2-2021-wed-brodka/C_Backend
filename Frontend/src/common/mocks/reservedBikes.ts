import { ReservedBike } from '../api/models/reservedBike';
import { StationStatus } from '../api/models/station';

export const mockedReservedBikes: ReservedBike[] = [
  {
    id: '54',
    station: {
      id: '1',
      name: 'Górczewska 100',
      status: StationStatus.Active,
      activeBikesCount: 3,
    },
    reservedAt: new Date(2020, 3, 10, 21, 37, 42),
    reservedTill: new Date(2022, 4, 10, 14, 34, 56),
  },
  {
    id: '55',
    station: {
      id: '2',
      name: 'Rondo Daszyńskiego',
      status: StationStatus.Active,
      activeBikesCount: 3,
    },
    reservedAt: new Date(2020, 3, 10, 21, 37, 42),
    reservedTill: new Date(2021, 11, 13, 14, 34, 56),
  },
  {
    id: '59',
    station: {
      id: '2',
      name: 'Rondo Daszyńskiego',
      status: StationStatus.Active,
      activeBikesCount: 3,
    },
    reservedAt: new Date(2020, 3, 10, 21, 37, 42),
    reservedTill: new Date(2021, 12, 23, 22, 11, 56),
  },
];
