import { createContext, useContext } from 'react';
import {
  addBike,
  addStation,
  blockBike,
  blockStation,
  getBikesByStation,
  getRentedBikes,
  getActiveStations,
  getAllStations,
  removeBike,
  removeStation,
  rentBike,
  returnBike,
  unblockBike,
  unblockStation,
  getReservedBikes,
  removeReservation,
  getUsers,
} from './api/endpoints';
import { Bike, BikeStatus } from './api/models/bike';
import { Station, StationStatus } from './api/models/station';
import { mockedStations } from './mocks/stations';
import { mockedBikesByStations } from './mocks/bikes';
import { mockedRentedBikes } from './mocks/rentals';
import { delay } from './mocks/mockedApiResponse';
import { BearerToken } from './api/models/bearer-token';
import {
  signInAndSaveToken,
  saveTokenInLocalStorage,
  signUpAndSaveToken,
} from './authentication/token-functions';
import { ReservedBike } from './api/models/reservedBike';
import { mockedReservedBikes } from './mocks/reservedBikes';
import { LoginResponse, UserRole } from './api/models/login-response';
import { User } from './api/models/user';
import { mockedUsers } from './mocks/users';

type AllServices = {
  signIn: (login: string, password: string) => Promise<LoginResponse>;
  signUp: (login: string, password: string) => Promise<BearerToken>;
  getActiveStations: () => Promise<Station[]>;
  getAllStations: () => Promise<Station[]>;
  getBikesOnStation: (stationId: string) => Promise<Bike[]>;
  getRentedBikes: () => Promise<Bike[]>;
  getReservedBikes: () => Promise<ReservedBike[]>;
  returnBike: (stationId: string, bikeId: string) => Promise<Bike>;
  rentBike: (bikeId: string) => Promise<Bike>;
  addStation: (name: string) => Promise<Station>;
  addBike: (stationId: string) => Promise<Bike>;
  removeBike: (bikeId: string) => Promise<void>;
  removeReservation: (bikeId: string) => Promise<void>;
  removeStation: (stationId: string) => Promise<void>;
  blockStation: (id: string) => Promise<Station>;
  blockBike: (id: string) => Promise<Bike>;
  unblockBike: (id: string) => Promise<void>;
  unblockStation: (id: string) => Promise<void>;
  getUsers: () => Promise<User[]>;
};

export const services: AllServices = {
  signIn: signInAndSaveToken,
  signUp: signUpAndSaveToken,
  getActiveStations: getActiveStations,
  getAllStations: getAllStations,
  getBikesOnStation: getBikesByStation,
  getRentedBikes: getRentedBikes,
  getReservedBikes: getReservedBikes,
  returnBike: returnBike,
  rentBike: rentBike,
  addStation: addStation,
  addBike: addBike,
  removeBike: removeBike,
  removeReservation: removeReservation,
  removeStation: removeStation,
  blockStation: blockStation,
  blockBike: blockBike,
  unblockBike: unblockBike,
  unblockStation: unblockStation,
  getUsers: getUsers,
};

export const mockedServices: AllServices = {
  signIn: login => {
    return delay({ token: login, role: UserRole.User }).then(user => {
      saveTokenInLocalStorage({ token: user.token });
      return user;
    });
  },
  signUp: login => {
    return delay({ token: login }).then(bearerToken => {
      saveTokenInLocalStorage(bearerToken);
      return bearerToken;
    });
  },
  getActiveStations: () => delay(mockedStations),
  getAllStations: () => delay(mockedStations),
  getBikesOnStation: stationId => delay(mockedBikesByStations[stationId]),
  getRentedBikes: () => delay(mockedRentedBikes),
  getReservedBikes: () => delay(mockedReservedBikes),
  returnBike: bikeId =>
    delay<Bike>({ id: bikeId, status: BikeStatus.Available }),
  rentBike: bikeId => delay<Bike>({ id: bikeId, status: BikeStatus.Rented }),
  addStation: name =>
    delay<Station>({
      id: '1',
      status: StationStatus.Active,
      activeBikesCount: 0,
      name,
    }),
  addBike: () => delay<Bike>({ id: '1', status: BikeStatus.Available }),
  removeBike: () => delay<void>(undefined),
  removeReservation: () => delay<void>(undefined),
  removeStation: () => delay<void>(undefined),
  blockStation: id =>
    delay<Station>({
      id,
      status: StationStatus.Active,
      activeBikesCount: 0,
      name: '',
    }),
  blockBike: id => delay<Bike>({ id, status: BikeStatus.Blocked }),
  unblockBike: () => delay<void>(undefined),
  unblockStation: () => delay<void>(undefined),
  getUsers: () => delay(mockedUsers),
};

export const ServicesContext = createContext(services);
export const useServices = () => useContext(ServicesContext);
