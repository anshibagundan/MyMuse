// atoms/userAtom.ts
import { atom } from 'jotai';
import { User } from '@/types/auth';

export const userAtom = atom<User | null>(null);
export const isLoadingAtom = atom<boolean>(true);