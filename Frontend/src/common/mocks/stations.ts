import { Station, StationStatus } from './../api/models/station';

export const mockedStations: Station[] = [
  {
    id: '1',
    name: 'Górczewska 100',
    activeBikesCount: 3,
    status: StationStatus.Active,
  },
  {
    id: '2',
    name: 'Puławska 23C',
    activeBikesCount: 3,
    status: StationStatus.Active,
  },
  {
    id: '3',
    name: 'Aleje Jerozolimskie 126',
    activeBikesCount: 3,
    status: StationStatus.Active,
  },
  {
    id: '4',
    name: 'Towarowa 78',
    activeBikesCount: 3,
    status: StationStatus.Active,
  },
  {
    id: '5',
    name: 'Koszykowa 44',
    activeBikesCount: 3,
    status: StationStatus.Active,
  },
  {
    id: '6',
    name: 'Rondo Daszyńskiego',
    activeBikesCount: 3,
    status: StationStatus.Active,
  },
];
