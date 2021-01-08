import { Ad } from './ad';

export interface User {
  id: number;
  userName: string;
  password: string;
  email: string;
  fullName: string;
  address: string;
  lastActive: Date;
  createdOn: Date;
  ads?: Ad[];
  roles?: string[];
}
