import useApi, { DataSource } from '../hooks/useApi';
import { Station } from './models/station';
import { Bike } from './models/bike';
import apiConnection from './api-connection';

const API = 'http://localhost:5000';

const mapResponse: <T, U>(
  res: DataSource<T>,
  map: (x: T) => U,
) => DataSource<U> = (res, map) => {
  return { ...res, results: res.results ? map(res.results) : null };
};

export type StationsResponse = { stations: Station[] };
export type BikesResponse = { bikes: Bike[] };

export const useStations = () =>
  mapResponse(useApi<StationsResponse>(`${API}/stations`), res => res.stations);

export const useBikesByStation = (stationId: string) =>
  mapResponse(
    useApi<BikesResponse>(`${API}/stations/${stationId}`),
    res => res.bikes,
  );

export const useRentedBikes = () =>
  mapResponse(useApi<BikesResponse>(`${API}/bikes/rented`), res => res.bikes);

export const returnBike = (bikeId: string) =>
  apiConnection(`${API}/bikes/rented`, { method: 'POST', data: { bikeId } });
