export enum StationStatus {
  Active = 'active',
  Blocked = 'blocked',
}

export interface Station {
  id: string;
  name: string;
  activeBikesCount: number;
  status: StationStatus;
}
