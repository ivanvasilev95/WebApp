import { Ad } from './ad';

export interface User {
  id: number;
  username: string;
  fullName: string;
  address: number;
  email: string;
  created: Date;
  lastActive: Date;
  ads?: Ad[];
}
