'use client';

import { useState } from 'react';
import { api } from "@/lib/api";
import { LoginData } from '@/types/user';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/hooks/useAuth';

export function EmailLoginButton() {
    const router = useRouter();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const { setUser } = useAuth();

    const handleLogin = async () => {
        const userData: LoginData = { email, password };
        try {
            const response = await api.login(userData);
            setUser(response); // Jotaiのatomに保存
            router.push('/home');
        } catch (error) {
            console.error('Login failed', error);
        }
    };

    return (
        <div>
            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="px-4 py-2 rounded-lg border mb-4 w-full"
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="px-4 py-2 rounded-lg border mb-4 w-full"
            />
            <button
                onClick={handleLogin}
                className="flex items-center justify-center gap-2 px-6 py-3 rounded-lg border hover:bg-gray-50 transition-colors"
            >
                <span>ログイン</span>
            </button>
        </div>
    );
}
