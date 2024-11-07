'use client';

import { FcGoogle } from 'react-icons/fc';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL || 'http://localhost:8080';

export function GmailLoginButton() {
    const handleLogin = () => {
        // ブラウザの cookie 設定を確認
        if (!navigator.cookieEnabled) {
            throw new Error('Cookies are disabled in your browser. Please enable cookies to continue.');
        }

        // Goバックエンドのログインエンドポイントにリダイレクト
        window.location.href = `${API_BASE_URL}/auth/google/login`;
    };

    return (
        <button
            onClick={handleLogin}
            className="flex items-center justify-center gap-2 px-6 py-3 rounded-lg border hover:bg-gray-50 transition-colors"
        >
            <FcGoogle size={24} />
            <span>Googleでログイン</span>
        </button>
    );
}
