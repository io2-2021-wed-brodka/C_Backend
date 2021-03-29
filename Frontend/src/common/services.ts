import { createContext, useContext } from 'react';
import { useBikesByStation, useStations } from './api/endpoints';
import { Bike } from './api/models/bike';
import { Station } from './api/models/station';
import { DataSource } from './hooks/useApi';

type AllServices = {
  useStations: () => DataSource<Station[]>;
  useBikesOnStation: (stationId: string) => DataSource<Bike[]>;
};

export const services: AllServices = {
  useStations: useStations,
  useBikesOnStation: useBikesByStation,
};

export const ServicesContext = createContext(services);
export const useServices = () => useContext(ServicesContext);
