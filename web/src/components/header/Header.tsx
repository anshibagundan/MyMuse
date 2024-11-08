'use client';

import {useEffect, useState} from 'react';
import { useAuth } from '@/hooks/useAuth';
import './header.css';
import Image from 'next/image';
import { useRouter } from 'next/navigation';

export function Header() {
    const router = useRouter();
    const { user} = useAuth();
    const [isSessionChecked, setIsSessionChecked] = useState(false);


    const handleNavigation = () => {
        if (user) {
            router.push('/home');
        } else {
            router.push('/');
        }
    };

    return (
        <header className="my-muse-header">
            <div className="container mx-auto flex justify-between items-center">
                <h1 className="my-muse-title" onClick={handleNavigation}>
                    My Muse
                </h1>
                {user ? (
                    <div className="flex items-center gap-4">
                        ログイン済み
                    </div>
                ) : (
                    <button
                        className="login-button"
                        onClick={() => router.push('/login')}
                    >
                        Login
                    </button>
                )}
            </div>
        </header>
    );
}
