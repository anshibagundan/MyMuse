// hooks/usePhotoGallery.ts
import { useState, useEffect } from 'react';
import { PhotoGallery } from '@/types/photoGallery';
import { api } from '@/lib/api';

export const usePhotoGallery = (userId: string | null) => {
    const [data, setData] = useState<PhotoGallery | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);

    useEffect(() => {
        const fetchGallery = async () => {
            if (!userId) {
                setData(null);
                setError(null);
                return;
            }

            try {
                setIsLoading(true);
                setError(null);
                const galleryData = await api.userData(userId);
                setData(galleryData);
            } catch (err) {
                setError(err instanceof Error ? err : new Error('Failed to fetch gallery data'));
                setData(null);
            } finally {
                setIsLoading(false);
            }
        };

        fetchGallery();
    }, [userId]);

    // Mutate function to update gallery data manually or re-fetch
    const mutate = async (userId: string | null) => {
        if (!userId) return;

        try {
            setIsLoading(true);
            const galleryData = await api.userData(userId);
            setData(galleryData);
        } catch (err) {
            setError(err instanceof Error ? err : new Error('Failed to fetch gallery data on mutate'));
            setData(null);
        } finally {
            setIsLoading(false);
        }
    };

    return { data, isLoading, error, mutate };
};
