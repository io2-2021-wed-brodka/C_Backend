import { Bike } from './../api/models/bike';

export const mockedBikesByStations: { [key: string]: Bike[] } = {
  '1': [
    { id: '125', status: 'available' },
    { id: '6544', status: 'available' },
    { id: '423', status: 'available' },
    { id: '3245', status: 'available' },
    { id: '786', status: 'available' },
  ],
  '2': [
    { id: '54', status: 'available' },
    { id: '566', status: 'available' },
    { id: '147', status: 'available' },
    { id: '9563', status: 'available' },
    { id: '244', status: 'available' },
    { id: '47', status: 'available' },
    { id: '235', status: 'available' },
    { id: '842', status: 'available' },
    { id: '2576', status: 'available' },
    { id: '34', status: 'available' },
    { id: '367', status: 'available' },
  ],
  '3': [
    { id: '5456', status: 'available' },
    { id: '435', status: 'available' },
    { id: '23', status: 'available' },
    { id: '4556', status: 'available' },
  ],
  '4': [],
  '5': [
    { id: '296', status: 'available' },
    { id: '3544', status: 'available' },
    { id: '2345', status: 'available' },
    { id: '205', status: 'available' },
    { id: '286', status: 'available' },
  ],
  '6': [
    { id: '12', status: 'available' },
    { id: '3234', status: 'available' },
  ],
};

export const mockedReservedBikes: Bike[] = [
  { id: '125', status: 'reserved' },
  { id: '6544', status: 'reserved' },
];
