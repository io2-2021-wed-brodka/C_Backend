import { Station } from './station';

export interface ReservedBike {
  id: string;
  station: Station;
  reservedAt: Date; // maybe string, not sure
  reservedTill: Date;
}
