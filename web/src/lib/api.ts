import axios from 'axios';
import {Session, User} from '@/types/auth';
import { PhotoGallery } from  '@/types/photoGallery';
import {CreateTagData, UpdateTagData} from '@/types/tag'
import {CreatePhotoData, UpdatePhotoData} from '@/types/photo'
import {LoginData} from "@/types/user";
import {useAtom} from "jotai/index";
import {isLoadingAtom, userAtom} from "@/atoms/userAtoms";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL;

const axiosInstance = axios.create({
    baseURL: API_BASE_URL,
    withCredentials: true, // Cookieを含める設定
});

export const api = {
    checkSession: async (): Promise<Session> => {
        const res = await axiosInstance.get('/auth/check-session');
        return res.data;
    },

    login: async (data: LoginData): Promise<User> => {
        const res = await axiosInstance.post("/auth/login", {
            email: data.email,
            password: data.password,
        });

        console.log(res.data)

        return res.data.user;
    },

    logout: async (): Promise<void> => {
        await axiosInstance.get('/auth/logout');
    },

    userData: async (userId: string): Promise<PhotoGallery> => {
        const res = await axiosInstance.get(`api/web/user/${userId}`)
        return res.data
    },

    createTag: async (data: CreateTagData): Promise<void> => {
        console.log("tag create now")
        await axiosInstance.post('/api/web/tag', data);
    },

    updateTag: async (data: UpdateTagData): Promise<void> => {
        await axiosInstance.put('/api/web/tag', data);
    },

    deleteTag: async (tagID: string): Promise<void> => {
        await axiosInstance.delete(`/api/web/tag/${tagID}`)
    },

    createPhoto: async (data: CreatePhotoData): Promise<void> => {
        await axiosInstance.post('/api/web/photo', data)
    },

    deletePhoto: async (photoID: string): Promise<void> => {
        await axiosInstance.delete(`/api/web/photo/${photoID}`)
    },

    updatePhoto: async (data: UpdatePhotoData): Promise<void> => {
        await axiosInstance.put(`/api/web/photo`, data)
    },

    updatePhotoOrder: async (tagId: string, updatedOrder: { id: number; order: number }[]): Promise<void> => {
        await axiosInstance.put(`/api/web/tag/${tagId}/photos/order`, { updatedOrder });
    }
};