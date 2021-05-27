import { Station } from './models/station';
import { Bike } from './models/bike';
import apiConnection from './api-connection';
import { BearerToken } from './models/bearer-token';
import { apiWithAuthConnection } from '../authentication/api-with-authentication';
import { ReservedBike } from './models/reservedBike';
import { LoginResponse } from './models/login-response';
import { User } from './models/user';
import { Tech } from './models/tech';
const API = 'http://localhost:8080';

export type StationsResponse = { stations: Station[] };
export type BikesResponse = { bikes: Bike[] };
export type ReservedBikesResponse = { bikes: ReservedBike[] };
export type UsersResponse = { users: User[] };
export type TechsResponse = { techs: Tech[] };

export const signIn = (login: string, password: string) =>
  apiConnection<LoginResponse>(`${API}/login`, {
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

export const getBikes = () =>
  apiWithAuthConnection<BikesResponse>(`${API}/bikes`).then(res => res.bikes);

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

export const getReservedBikes = () =>
  apiWithAuthConnection<ReservedBikesResponse>(`${API}/bikes/reserved`).then(
    res =>
      res.bikes.map(bike => ({
        ...bike,
        reservedTill: new Date(bike.reservedTill),
        reservedAt: new Date(bike.reservedAt),
      })),
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

export const reserveBike = (bikeId: string) =>
  apiWithAuthConnection<ReservedBike>(`${API}/bikes/reserved`, {
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

export const removeReservation = (bikeId: string) =>
  apiWithAuthConnection<void>(`${API}/bikes/reserved/${bikeId}`, {
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

export const blockUser = (id: string) =>
  apiWithAuthConnection<User>(`${API}/users/blocked`, {
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

export const unblockUser = (userId: string) =>
  apiWithAuthConnection<void>(`${API}/users/blocked/${userId}`, {
    method: 'DELETE',
  });

export const getUsers = () =>
  apiWithAuthConnection<UsersResponse>(`${API}/users`).then(res => res.users);

export const getBlockedUsers = () =>
  apiWithAuthConnection<UsersResponse>(`${API}/users/blocked`).then(
    res => res.users,
  );

export const getTechs = () =>
  apiWithAuthConnection<TechsResponse>(`${API}/techs`).then(res => res.techs);

export const removeTech = (id: string) =>
  apiWithAuthConnection<void>(`${API}/techs/${id}`, {
    method: 'DELETE',
  });

export const addTech = (name: string, password: string) =>
  apiWithAuthConnection<Tech>(`${API}/techs`, {
    method: 'POST',
    data: { name, password },
  });
