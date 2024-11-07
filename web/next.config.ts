import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
    images: {
        domains: ['lh3.googleusercontent.com'], // 外部画像ホスト名を追加
    },
    eslint: {
        ignoreDuringBuilds: true, // ビルド時のESLintチェックを無効化
    },
    async headers() {
        return [
            {
                source: '/:path*',
                headers: [
                    {
                        key: 'Access-Control-Allow-Credentials',
                        value: 'true',
                    },
                    {
                        key: 'Access-Control-Allow-Origin',
                        value: process.env.NEXT_PUBLIC_API_BASE_URL || '', // バックエンドのURL（環境変数）を設定
                    },
                ],
            },
        ];
    },
};

export default nextConfig;
