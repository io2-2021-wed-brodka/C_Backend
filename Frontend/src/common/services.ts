import { createContext, useContext } from 'react';
import {
  addBike,
  addStation,
  blockBike,
  blockStation,
  getBikesByStation,
  getRentedBikes,
  getStations,
  removeBike,
  removeStation,
  rentBike,
  returnBike,
  unblockBike,
  unblockStation,
} from './api/endpoints';
import { Bike } from './api/models/bike';
import { Station } from './api/models/station';
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

type AllServices = {
  signIn: (login: string, password: string) => Promise<BearerToken>;
  signUp: (login: string, password: string) => Promise<BearerToken>;
  getStations: () => Promise<Station[]>;
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
  getStations: getStations,
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
    return delay({ token: login }).then(bearerToken => {
      saveTokenInLocalStorage(bearerToken);
      return bearerToken;
    });
  },
  signUp: login => {
    return delay({ token: login }).then(bearerToken => {
      saveTokenInLocalStorage(bearerToken);
      return bearerToken;
    });
  },
  getStations: () => delay(mockedStations),
  getBikesOnStation: stationId => delay(mockedBikesByStations[stationId]),
  getRentedBikes: () => delay(mockedRentedBikes),
  returnBike: bikeId => delay<Bike>({ id: bikeId }),
  rentBike: bikeId => delay<Bike>({ id: bikeId }),
  addStation: name => delay<Station>({ id: '1', status: 'active', name }),
  addBike: () => delay<Bike>({ id: '1' }),
  removeBike: () => delay<void>(undefined),
  removeStation: () => delay<void>(undefined),
  blockStation: id => delay<Station>({ id, status: 'active', name: '' }),
  blockBike: id => delay<Bike>({ id }),
  unblockBike: () => delay<void>(undefined),
  unblockStation: () => delay<void>(undefined),
};

export const ServicesContext = createContext(services);
export const useServices = () => useContext(ServicesContext);
