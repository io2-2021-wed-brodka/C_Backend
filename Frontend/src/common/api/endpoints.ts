import { Station } from './models/station';
import { Bike } from './models/bike';
import apiConnection from './api-connection';
import { BearerToken } from './models/bearer-token';
import { apiWithAuthConnection } from '../authentication/api-with-authentication';
import { User } from './models/user';

const API = 'http://localhost:5000';

export type StationsResponse = { stations: Station[] };
export type BikesResponse = { bikes: Bike[] };

export const signIn = (login: string, password: string) =>
  apiConnection<User>(`${API}/login`, {
    method: 'POST',
    data: { login, password },
  });

export const signUp = (login: string, password: string) =>
  apiConnection<BearerToken>(`${API}/register`, {
    method: 'POST',
    data: { login, password },
  });

export const getActiveStations = () =>
  apiWithAuthConnection<StationsResponse>(`${API}/stations/active`).then(
    res => res.stations,
  );

export const getAllStations = () =>
  apiWithAuthConnection<StationsResponse>(`${API}/stations`).then(
    res => res.stations,
  );

export const getBikesByStation = (stationId: string) =>
  apiWithAuthConnection<BikesResponse>(
    `${API}/stations/${stationId}/bikes`,
  ).then(res => res.bikes);

export const getRentedBikes = () =>
  apiWithAuthConnection<BikesResponse>(`${API}/bikes/rented`).then(
    res => res.bikes,
  );

export const returnBike = (stationId: string, bikeId: string) =>
  apiWithAuthConnection<Bike>(`${API}/stations/${stationId}/bikes`, {
    method: 'POST',
    data: { id: bikeId },
  });

export const rentBike = (bikeId: string) =>
  apiWithAuthConnection<Bike>(`${API}/bikes/rented`, {
    method: 'POST',
    data: { id: bikeId },
  });

export const addStation = (name: string) =>
  apiWithAuthConnection<Station>(`${API}/stations`, {
    method: 'POST',
    data: { name },
  });

export const addBike = (stationId: string) =>
  apiWithAuthConnection<Bike>(`${API}/bikes`, {
    method: 'POST',
    data: { stationId },
  });

export const removeBike = (bikeId: string) =>
  apiWithAuthConnection<void>(`${API}/bikes/${bikeId}`, {
    method: 'DELETE',
  });

export const removeStation = (stationId: string) =>
  apiWithAuthConnection<void>(`${API}/stations/${stationId}`, {
    method: 'DELETE',
  });

export const blockStation = (id: string) =>
  apiWithAuthConnection<Station>(`${API}/stations/blocked`, {
    method: 'POST',
    data: { id },
  });

export const blockBike = (id: string) =>
  apiWithAuthConnection<Bike>(`${API}/bikes/blocked`, {
    method: 'POST',
    data: { id },
  });

export const unblockBike = (bikeId: string) =>
  apiWithAuthConnection<void>(`${API}/bikes/blocked/${bikeId}`, {
    method: 'DELETE',
  });

export const unblockStation = (stationId: string) =>
  apiWithAuthConnection<void>(`${API}/stations/blocked/${stationId}`, {
    method: 'DELETE',
  });
