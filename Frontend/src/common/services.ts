import { createContext, useContext } from 'react';
import {
  getBikesByStation,
  getRentedBikes,
  getStations,
  returnBike,
  signIn,
} from './api/endpoints';
import { Bike } from './api/models/bike';
import { Station } from './api/models/station';
import { mockedStations } from './mocks/stations';
import { mockedBikesByStations } from './mocks/bikes';
import { mockedRentedBikes } from './mocks/rentals';
import { delay } from './mocks/mockedApiResponse';
import { BearerToken } from './api/models/bearer-token';

type AllServices = {
  signIn: (login: string, password: string) => Promise<BearerToken>;
  getStations: () => Promise<Station[]>;
  getBikesOnStation: (stationId: string) => Promise<Bike[]>;
  getRentedBikes: () => Promise<Bike[]>;
  returnBike: (stationId: string, bikeId: string) => Promise<Bike>;
};

export const services: AllServices = {
  signIn: signIn,
  getStations: getStations,
  getBikesOnStation: getBikesByStation,
  getRentedBikes: getRentedBikes,
  returnBike: returnBike,
};

export const mockedServices: AllServices = {
  signIn: login => delay({ token: login }),
  getStations: () => delay(mockedStations),
  getBikesOnStation: stationId => delay(mockedBikesByStations[stationId]),
  getRentedBikes: () => delay(mockedRentedBikes),
  returnBike: bikeId => delay<Bike>({ id: bikeId }),
};

export const ServicesContext = createContext(services);
export const useServices = () => useContext(ServicesContext);
