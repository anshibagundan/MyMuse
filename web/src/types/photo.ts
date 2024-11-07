export interface CreatePhotoData {
    title:string;
    detailed_title: string;
    content:string;
    height: number;
    width: number;
    order: number;
    tag_id: number;
}

export interface UpdatePhotoData {
    id: number
    title:string;
    detailed_title: string;
}