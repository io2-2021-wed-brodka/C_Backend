import useApi from '../hooks/useApi';
import { Station } from './models/station';
import { Bike } from './models/bike';

const API = 'http://localhost:5000';

export type StationsList = { stations: Station[] };
export type BikesList = { bikes: Bike[] };

export const useStations = () => useApi<StationsList>(`${API}/stations`);

export const useBikesByStation = (stationId: string) =>
  useApi<BikesList>(`${API}/stations/${stationId}`);
