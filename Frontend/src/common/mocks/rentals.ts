import { Bike, BikeStatus } from './../api/models/bike';

export const mockedRentedBikes: Bike[] = [
  { id: '126', status: BikeStatus.Rented },
  { id: '6504', status: BikeStatus.Rented },
  { id: '420', status: BikeStatus.Rented },
];
