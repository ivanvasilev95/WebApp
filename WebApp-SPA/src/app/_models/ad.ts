import { Photo } from './photo';

export interface Ad {
    id: number;
    title: string;
    location: string;
    price: number;
    dateAdded: Date;
    photoUrl: string;
    description?: string;
    isUsed?: boolean;
    userId?: number;
    photos?: Photo[];
}
