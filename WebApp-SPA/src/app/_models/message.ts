export interface Message {
    id: number;
    senderId: number;
    senderUsername: string;
    recipientId: number;
    recipientUsername: string;
    adId: number;
    adTitle: string;
    content: string;
    isRead: boolean;
    dateRead: Date;
    messageSent: Date;
    senderDeleted: boolean;
    recipientDeleted: boolean;
}
