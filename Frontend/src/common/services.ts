import { createContext, useContext } from 'react';
import {
  BikesList,
  StationsList,
  useBikesByStation,
  useStations,
} from './api/endpoints';
import { DataSource } from './hooks/useApi';

type AllServices = {
  useStations: () => DataSource<StationsList>;
  useBikesOnStation: (stationId: string) => DataSource<BikesList>;
};

export const services: AllServices = {
  useStations: useStations,
  useBikesOnStation: useBikesByStation,
};

export const ServicesContext = createContext(services);
export const useServices = () => useContext(ServicesContext);
