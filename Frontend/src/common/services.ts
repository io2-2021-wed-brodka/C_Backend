import { createContext, useContext } from 'react';
import {
  addBike,
  addStation,
  blockBike,
  blockStation,
  blockUser,
  getBikes,
  getBikesByStation,
  getReservedBikes,
  getRentedBikes,
  getActiveStations,
  getAllStations,
  removeBike,
  removeStation,
  rentBike,
  reserveBike,
  returnBike,
  unblockBike,
  unblockStation,
  unblockUser,
  removeReservation,
  getUsers,
  getBlockedUsers,
  getTechs,
  removeTech,
  addTech,
  getMalfunctions,
  addMalfunction,
  removeMalfunction,
} from './api/endpoints';
import { Bike, BikeStatus } from './api/models/bike';
import { Station, StationStatus } from './api/models/station';
import { mockedStations } from './mocks/stations';
import { mockedBikesByStations, mockedBikes } from './mocks/bikes';
import { mockedRentedBikes } from './mocks/rentals';
import { delay } from './mocks/mockedApiResponse';
import { BearerToken } from './api/models/bearer-token';
import {
  signInAndSaveToken,
  saveUserDataInLocalStorage,
  signUpAndSaveToken,
  getLoginFromLocalStorage,
  getRoleFromLocalStorage,
} from './authentication/token-functions';
import { ReservedBike } from './api/models/reservedBike';
import { mockedReservedBikes } from './mocks/reservedBikes';
import { LoginResponse, UserRole } from './api/models/login-response';
import { User } from './api/models/user';
import { mockedUsers } from './mocks/users';
import { Tech } from './api/models/tech';
import { mockedTechs } from './mocks/techs';
import { Malfunction } from './api/models/malfunction';
import { mockedMalfunctions } from './mocks/malfunctions';

type AllServices = {
  signIn: (login: string, password: string) => Promise<LoginResponse>;
  signUp: (login: string, password: string) => Promise<BearerToken>;
  getLogin: () => Promise<string>;
  getRole: () => Promise<UserRole | null>;
  getActiveStations: () => Promise<Station[]>;
  getAllStations: () => Promise<Station[]>;
  getBikes: () => Promise<Bike[]>;
  getBikesOnStation: (stationId: string) => Promise<Bike[]>;
  getRentedBikes: () => Promise<Bike[]>;
  getReservedBikes: () => Promise<ReservedBike[]>;
  returnBike: (stationId: string, bikeId: string) => Promise<Bike>;
  rentBike: (bikeId: string) => Promise<Bike>;
  reserveBike: (bikeId: string) => Promise<ReservedBike>;
  addStation: (name: string, bikesLimit?: number) => Promise<Station>;
  addBike: (stationId: string) => Promise<Bike>;
  removeBike: (bikeId: string) => Promise<void>;
  removeReservation: (bikeId: string) => Promise<void>;
  removeStation: (stationId: string) => Promise<void>;
  blockStation: (id: string) => Promise<Station>;
  blockBike: (id: string) => Promise<Bike>;
  blockUser: (id: string) => Promise<User>;
  unblockBike: (id: string) => Promise<void>;
  unblockStation: (id: string) => Promise<void>;
  unblockUser: (id: string) => Promise<void>;
  getUsers: () => Promise<User[]>;
  getBlockedUsers: () => Promise<User[]>;
  getTechs: () => Promise<Tech[]>;
  removeTech: (id: string) => Promise<void>;
  addTech: (name: string, password: string) => Promise<Tech>;
  getMalfunctions: () => Promise<Malfunction[]>;
  addMalfunction: (bikeId: string, description: string) => Promise<Malfunction>;
  removeMalfunction: (id: string) => Promise<void>;
};

const getAllStationsWithCounts = async () => {
  const [stations, malfunctions, bikes] = await Promise.all([
    getAllStations(),
    getMalfunctions(),
    getBikes(),
  ]);

  return stations.map(station => ({
    ...station,
    malfunctionsCount: bikes
      .filter(b => b.station?.id == station.id)
      .filter(b => malfunctions.some(m => m.bikeId == b.id)).length,
    reservationsCount: bikes
      .filter(b => b.station?.id == station.id)
      .filter(b => b.status == BikeStatus.Reserved).length,
  }));
};

const getMalfunctionsWithStatuses = async () => {
  const [malfunctions, bikes] = await Promise.all([
    getMalfunctions(),
    getBikes(),
  ]);

  return malfunctions.map(malfunction => ({
    ...malfunction,
    isBeingFixed: bikes.some(
      b => b.id == malfunction.bikeId && b.status == BikeStatus.Blocked,
    ),
  }));
};

export const services: AllServices = {
  signIn: signInAndSaveToken,
  signUp: signUpAndSaveToken,
  getLogin: getLoginFromLocalStorage,
  getRole: getRoleFromLocalStorage,
  getActiveStations: getActiveStations,
  getAllStations: getAllStationsWithCounts,
  getBikes: getBikes,
  getBikesOnStation: getBikesByStation,
  getReservedBikes: getReservedBikes,
  getRentedBikes: getRentedBikes,
  returnBike: returnBike,
  rentBike: rentBike,
  reserveBike: reserveBike,
  addStation: addStation,
  addBike: addBike,
  removeBike: removeBike,
  removeReservation: removeReservation,
  removeStation: removeStation,
  blockStation: blockStation,
  blockBike: blockBike,
  blockUser: blockUser,
  unblockBike: unblockBike,
  unblockStation: unblockStation,
  unblockUser: unblockUser,
  getUsers: getUsers,
  getBlockedUsers: getBlockedUsers,
  getTechs: getTechs,
  removeTech: removeTech,
  addTech: addTech,
  getMalfunctions: getMalfunctionsWithStatuses,
  addMalfunction: addMalfunction,
  removeMalfunction: removeMalfunction,
};

export const mockedServices: AllServices = {
  signIn: login => {
    return delay({ token: login, role: UserRole.User }).then(user => {
      saveUserDataInLocalStorage(user.token, login, UserRole.User);
      return user;
    });
  },
  signUp: login => {
    return delay({ token: login }).then(bearerToken => {
      saveUserDataInLocalStorage(bearerToken.token, login, UserRole.User);
      return bearerToken;
    });
  },
  getLogin: getLoginFromLocalStorage,
  getRole: getRoleFromLocalStorage,
  getActiveStations: () => delay(mockedStations),
  getAllStations: () => delay(mockedStations),
  getBikes: () => delay(mockedBikes),
  getBikesOnStation: stationId => delay(mockedBikesByStations[stationId]),
  getRentedBikes: () => delay(mockedRentedBikes),
  getReservedBikes: () => delay(mockedReservedBikes),
  returnBike: bikeId =>
    delay<Bike>({ id: bikeId, status: BikeStatus.Available }),
  rentBike: bikeId => delay<Bike>({ id: bikeId, status: BikeStatus.Rented }),
  addStation: name =>
    delay<Station>({
      id: '1',
      status: StationStatus.Active,
      activeBikesCount: 0,
      name,
    }),
  addBike: () => delay<Bike>({ id: '1', status: BikeStatus.Available }),
  removeBike: () => delay<void>(undefined),
  reserveBike: bikeId =>
    delay<ReservedBike>({
      id: bikeId,
      station: mockedReservedBikes[0].station,
      reservedAt: mockedReservedBikes[0].reservedAt,
      reservedTill: mockedReservedBikes[0].reservedTill,
    }),
  removeReservation: () => delay<void>(undefined),
  removeStation: () => delay<void>(undefined),
  blockStation: id =>
    delay<Station>({
      id,
      status: StationStatus.Active,
      activeBikesCount: 0,
      name: '',
    }),
  blockBike: id => delay<Bike>({ id, status: BikeStatus.Blocked }),
  blockUser: id => delay<User>({ id, name: '' }),
  unblockBike: () => delay<void>(undefined),
  unblockStation: () => delay<void>(undefined),
  unblockUser: () => delay<void>(undefined),
  getUsers: () => delay(mockedUsers),
  getBlockedUsers: () => delay(mockedUsers),
  getTechs: () => delay(mockedTechs),
  removeTech: () => delay<void>(undefined),
  addTech: (name: string) => delay<Tech>({ id: '666', name }),
  getMalfunctions: () => delay<Malfunction[]>(mockedMalfunctions),
  addMalfunction: (bikeId, description) =>
    delay<Malfunction>({ id: '1', reportingUserId: '1', bikeId, description }),
  removeMalfunction: () => delay<void>(undefined),
};

export const ServicesContext = createContext(services);
export const useServices = () => useContext(ServicesContext);
