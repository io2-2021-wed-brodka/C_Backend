import { Station } from './models/station';
import { Bike } from './models/bike';
import apiConnection from './api-connection';
import { BearerToken } from './models/bearer-token';

const API = 'http://localhost:5000';

export type StationsResponse = { stations: Station[] };
export type BikesResponse = { bikes: Bike[] };

export const signIn = (login: string, password: string) =>
  apiConnection<BearerToken>(`${API}/login`, {
    method: 'POST',
    data: { login, password },
  });

export const getStations = () =>
  apiConnection<StationsResponse>(`${API}/stations`).then(res => res.stations);

export const getBikesByStation = (stationId: string) =>
  apiConnection<BikesResponse>(`${API}/stations/${stationId}/bikes`).then(
    res => res.bikes,
  );

export const getRentedBikes = () =>
  apiConnection<BikesResponse>(`${API}/bikes/rented`).then(res => res.bikes);

export const returnBike = (stationId: string, bikeId: string) =>
  apiConnection<Bike>(`${API}/stations/${stationId}/bikes`, {
    method: 'POST',
    data: { id: bikeId },
  });
