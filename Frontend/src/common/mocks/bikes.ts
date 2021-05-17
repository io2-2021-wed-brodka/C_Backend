import { Bike, BikeStatus } from './../api/models/bike';

export const mockedBikesByStations: { [key: string]: Bike[] } = {
  '1': [
    { id: '125', status: BikeStatus.Available },
    { id: '6544', status: BikeStatus.Available },
    { id: '423', status: BikeStatus.Available },
    { id: '3245', status: BikeStatus.Available },
    { id: '786', status: BikeStatus.Available },
  ],
  '2': [
    { id: '54', status: BikeStatus.Available },
    { id: '566', status: BikeStatus.Available },
    { id: '147', status: BikeStatus.Available },
    { id: '9563', status: BikeStatus.Available },
    { id: '244', status: BikeStatus.Available },
    { id: '47', status: BikeStatus.Available },
    { id: '235', status: BikeStatus.Available },
    { id: '842', status: BikeStatus.Available },
    { id: '2576', status: BikeStatus.Available },
    { id: '34', status: BikeStatus.Available },
    { id: '367', status: BikeStatus.Available },
  ],
  '3': [
    { id: '5456', status: BikeStatus.Available },
    { id: '435', status: BikeStatus.Available },
    { id: '23', status: BikeStatus.Available },
    { id: '4556', status: BikeStatus.Available },
  ],
  '4': [],
  '5': [
    { id: '296', status: BikeStatus.Available },
    { id: '3544', status: BikeStatus.Available },
    { id: '2345', status: BikeStatus.Available },
    { id: '205', status: BikeStatus.Available },
    { id: '286', status: BikeStatus.Available },
  ],
  '6': [
    { id: '12', status: BikeStatus.Available },
    { id: '3234', status: BikeStatus.Available },
  ],
};
