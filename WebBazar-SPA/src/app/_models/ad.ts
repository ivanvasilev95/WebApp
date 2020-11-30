import { Photo } from './photo';

export interface Ad {
    id: number;
    title: string;
    location: string;
    price?: number;
    dateAdded: Date;
    photoUrl: string;
    categoryId?: number;
    categoryName?: string;
    description?: string;
    isUsed?: boolean;
    userId?: number;
    userName?: string;
    photos?: Photo[];
    isApproved: boolean;
    likesCount?: number;
}
