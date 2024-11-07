import { useAtom } from 'jotai';
import { userAtom } from '@/atoms/userAtoms';

export function useAuth() {
    const [user, setUser] = useAtom(userAtom);
    return { user, setUser };
}