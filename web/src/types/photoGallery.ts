export interface Photo {
    ID: number;
    Title: string;
    DetailedTitle: string;
    Content: string;
}

export interface PhotoTag {
    ID: string
    name: string;
    roomType: string;
    photos: Photo[];
}

export interface PhotoGallery {
    tags: PhotoTag[];
}