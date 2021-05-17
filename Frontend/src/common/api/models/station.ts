export interface Station {
  id: string;
  name: string;
  status: 'active' | 'blocked';
  activeBikesCount: number;
}
