export interface Malfunction {
  id: string;
  bikeId: string;
  description: string;
  reportingUserId: string;
  isBeingFixed?: boolean;
}
