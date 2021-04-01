import { createContext, useContext } from 'react';
import {
  useBikesByStation,
  useStations,
  useRentedBikes,
} from './api/endpoints';
import { Bike } from './api/models/bike';
import { Station } from './api/models/station';
import { DataSource } from './hooks/useApi';
import useMockedApi from './hooks/useMockedApi';
import { mockedStations } from './mocks/stations';
import { mockedBikesByStations } from './mocks/bikes';
import { mockedRentedBikes } from './mocks/rentals';

type AllServices = {
  useStations: () => DataSource<Station[]>;
  useBikesOnStation: (stationId: string) => DataSource<Bike[]>;
  useRentedBikes: () => DataSource<Bike[]>;
};

export const services: AllServices = {
  useStations: useStations,
  useBikesOnStation: useBikesByStation,
  useRentedBikes: useRentedBikes,
};

export const mockedServices: AllServices = {
  useStations: () => useMockedApi(mockedStations),
  useBikesOnStation: stationId =>
    useMockedApi(mockedBikesByStations[stationId]),
  useRentedBikes: () => useMockedApi(mockedRentedBikes),
};

export const ServicesContext = createContext(services);
export const useServices = () => useContext(ServicesContext);
