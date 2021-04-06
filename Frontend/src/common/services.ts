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

type AllServices = {
  getStations: () => Promise<Station[]>;
  getBikesOnStation: (stationId: string) => Promise<Bike[]>;
  getRentedBikes: () => Promise<Bike[]>;
  returnBike: (bikeId: string) => Promise<Bike>;
};

export const services: AllServices = {
  getStations: getStations,
  getBikesOnStation: getBikesByStation,
  getRentedBikes: getRentedBikes,
  returnBike: returnBike,
};

export const mockedServices: AllServices = {
  getStations: () => delay(mockedStations),
  getBikesOnStation: stationId => delay(mockedBikesByStations[stationId]),
  getRentedBikes: () => delay(mockedRentedBikes),
  returnBike: (bikeId: string) => delay<Bike>({ id: bikeId }),
};

export const ServicesContext = createContext(services);
export const useServices = () => useContext(ServicesContext);
