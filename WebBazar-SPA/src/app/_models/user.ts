import { Ad } from './ad';

export interface User {
  id: number;
  userName: string;
  password: string;
  fullName: string;
  address: string;
  email: string;
  created: Date;
  lastActive: Date;
  ads?: Ad[];
  roles?: string[];
}