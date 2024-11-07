import { PhotoEdit } from "@/components/photo/PhotoUpdateForm";
import { PhotoDeleteForm } from "@/components/photo/PhotoDeleteForm";
import {Photo} from "@/types/photoGallery";

interface PhotoCardInterface{
    photo: Photo
    onDeleteSuccess: () => void;
}

export const PhotoCard = ({ photo, onDeleteSuccess }: PhotoCardInterface) => {
    return (
        <div
            key={`photo_${photo.ID}`}
            data-id={photo.ID}
            className="flex-shrink-0 w-1/4 border rounded-lg p-4 shadow hover:shadow-lg transition-shadow cursor-move"
            style={{backgroundColor: 'rgba(255, 255, 255, 0.8)', minWidth: '220px'}}
        >
            <h3 className="font-semibold text-lg mb-2">{photo.Title}</h3>
            {photo.DetailedTitle && <p className="text-sm text-gray-600 mb-2">{photo.DetailedTitle}</p>}
            <img src={photo.Content} alt="Photo" className="max-w-full h-auto"/>
            <div style={{padding: '5px', display: 'flex', gap: '5px'}}>
                <PhotoEdit photoId={photo.ID} currentTitle={photo.Title} currentDetailedTitle={photo.DetailedTitle}
                           onSuccess={onDeleteSuccess}/>
                <PhotoDeleteForm photoId={photo.ID} onDeleteSuccess={onDeleteSuccess}/>
            </div>
        </div>
    );
};