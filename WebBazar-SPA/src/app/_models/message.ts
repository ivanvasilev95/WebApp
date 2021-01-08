export interface Message {
    id: number;
    adId: number;
    adTitle: string;
    senderId: number;
    senderUsername: string;
    senderDeleted: boolean;
    recipientId: number;
    recipientUsername: string;
    // recipientDeleted: boolean;
    content: string;
    sentOn: Date;
    isRead: boolean;
    // readOn?: Date;
}
