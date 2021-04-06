import { Station } from './models/station';
import { Bike } from './models/bike';
import apiConnection from './api-connection';

const API = 'http://localhost:5000';

export type StationsResponse = { stations: Station[] };
export type BikesResponse = { bikes: Bike[] };

export const getStations = () =>
  apiConnection<StationsResponse>(`${API}/stations`).then(res => res.stations);

export const getBikesByStation = (stationId: string) =>
  apiConnection<BikesResponse>(`${API}/stations/${stationId}`).then(
    res => res.bikes,
  );

export const getRentedBikes = () =>
  apiConnection<BikesResponse>(`${API}/bikes/rented`).then(res => res.bikes);

export const returnBike = (bikeId: string) =>
  apiConnection<Bike>(`${API}/bikes/rented`, {
    method: 'POST',
    data: { bikeId },
  });
