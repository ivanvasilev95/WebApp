import { Photo } from './photo';

export interface Ad {
    id: number;
    title: string;
    description?: string;
    location: string;
    photoUrl: string;
    categoryId?: number;
    categoryName?: string;
    userId?: number;
    userName?: string;
    isUsed?: boolean;
    price?: number;
    isApproved: boolean;
    dateAdded: Date;
    likesCount?: number;
    photos?: Photo[];
}
