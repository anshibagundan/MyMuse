export interface CreateTagData {
    name: string;
    roomType: string;
    user_id: string;
    order: number;
}

export interface UpdateTagData {
    id: number
    name: string;
    roomType: string;
}