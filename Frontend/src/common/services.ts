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
} from './api/endpoints';
import { Bike, BikeStatus } from './api/models/bike';
import { Station, StationStatus } from './api/models/station';
import { mockedStations } from './mocks/stations';
import { mockedBikesByStations } from './mocks/bikes';
import { mockedRentedBikes } from './mocks/rentals';
import { delay } from './mocks/mockedApiResponse';
import { BearerToken } from './api/models/bearer-token';
import { signInAndSaveToken, saveTokenInLocalStorage, signUpAndSaveToken } from './authentication/token-functions';
import { User, UserRole } from './api/models/user';

type AllServices = {
  signIn: (login: string, password: string) => Promise<User>;
  signUp: (login: string, password: string) => Promise<BearerToken>;
  getActiveStations: () => Promise<Station[]>;
  getAllStations: () => Promise<Station[]>;
  getBikesOnStation: (stationId: string) => Promise<Bike[]>;
  getRentedBikes: () => Promise<Bike[]>;
  returnBike: (stationId: string, bikeId: string) => Promise<Bike>;
  rentBike: (bikeId: string) => Promise<Bike>;
  addStation: (name: string) => Promise<Station>;
  addBike: (stationId: string) => Promise<Bike>;
  removeBike: (bikeId: string) => Promise<void>;
  removeStation: (stationId: string) => Promise<void>;
  blockStation: (id: string) => Promise<Station>;
  blockBike: (id: string) => Promise<Bike>;
  unblockBike: (id: string) => Promise<void>;
  unblockStation: (id: string) => Promise<void>;
};

export const services: AllServices = {
  signIn: signInAndSaveToken,
  signUp: signUpAndSaveToken,
  getActiveStations: getActiveStations,
  getAllStations: getAllStations,
  getBikesOnStation: getBikesByStation,
  getRentedBikes: getRentedBikes,
  returnBike: returnBike,
  rentBike: rentBike,
  addStation: addStation,
  addBike: addBike,
  removeBike: removeBike,
  removeStation: removeStation,
  blockStation: blockStation,
  blockBike: blockBike,
  unblockBike: unblockBike,
  unblockStation: unblockStation,
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
  returnBike: bikeId => delay<Bike>({ id: bikeId, status: BikeStatus.Available }),
  rentBike: bikeId => delay<Bike>({ id: bikeId, status: BikeStatus.Rented }),
  addStation: name => delay<Station>({ id: '1', status: StationStatus.Active, name }),
  addBike: () => delay<Bike>({ id: '1', status: BikeStatus.Available }),
  removeBike: () => delay<void>(undefined),
  removeStation: () => delay<void>(undefined),
  blockStation: id => delay<Station>({ id, status: StationStatus.Active, name: '' }),
  blockBike: id => delay<Bike>({ id, status: BikeStatus.Blocked }),
  unblockBike: () => delay<void>(undefined),
  unblockStation: () => delay<void>(undefined),
};

export const ServicesContext = createContext(services);
export const useServices = () => useContext(ServicesContext);
