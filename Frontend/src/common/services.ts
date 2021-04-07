import { createContext, useContext } from 'react';
import {
  getBikesByStation,
  getRentedBikes,
  getStations,
  returnBike,
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
} from './authentication/token-functions';

type AllServices = {
  signIn: (login: string, password: string) => Promise<BearerToken>;
  getStations: () => Promise<Station[]>;
  getBikesOnStation: (stationId: string) => Promise<Bike[]>;
  getRentedBikes: () => Promise<Bike[]>;
  returnBike: (stationId: string, bikeId: string) => Promise<Bike>;
};

export const services: AllServices = {
  signIn: signInAndSaveToken,
  getStations: getStations,
  getBikesOnStation: getBikesByStation,
  getRentedBikes: getRentedBikes,
  returnBike: returnBike,
};

export const mockedServices: AllServices = {
  signIn: login => {
    return delay(
      { token: login },
      Math.random() < 0.02 ? new Error('Wrong credentials') : undefined,
    ).then(bearerToken => {
      saveTokenInLocalStorage(bearerToken);
      return bearerToken;
    });
  },
  getStations: () => delay(mockedStations),
  getBikesOnStation: stationId => delay(mockedBikesByStations[stationId]),
  getRentedBikes: () => delay(mockedRentedBikes),
  returnBike: bikeId => delay<Bike>({ id: bikeId }),
};

export const ServicesContext = createContext(services);
export const useServices = () => useContext(ServicesContext);
