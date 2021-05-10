export enum StationStatus {
  Active = 'Active',
  Blocked = 'Blocked',
}

export interface Station {
  id: string;
  name: string;
  status: StationStatus;
}
