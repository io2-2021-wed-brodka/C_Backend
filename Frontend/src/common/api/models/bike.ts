export enum BikeStatus {
  Available = 'Available',
  Blocked = 'Blocked',
  Rented = 'Rented',
  Reserved = 'Reserved',
}

export interface Bike {
  id: string;
  status: BikeStatus;
}
