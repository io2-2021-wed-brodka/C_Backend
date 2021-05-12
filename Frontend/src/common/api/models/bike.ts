export enum BikeStatus {
  Available = 'available',
  Blocked = 'blocked',
  Rented = 'rented',
  Reserved = 'reserved',
}

export interface Bike {
  id: string;
  status: BikeStatus;
}
