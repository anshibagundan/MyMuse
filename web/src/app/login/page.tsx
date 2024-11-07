import { GmailLoginButton } from '@/components/login/GmailLoginButton';
import { headers } from 'next/headers';
import { redirect } from 'next/navigation';
import {EmailLoginButton} from "@/components/login/EmailLoginButton";

export default async function LoginPage() {
    // サーバーサイドでセッションチェック

    return (
        <div className="min-h-screen flex items-center justify-center">
            <div className="p-8 bg-white rounded-lg shadow-md" style={{width: '75vw', height: '50vh'}}>
                <h1 className="text-2xl font-bold text-center mb-6">ログイン</h1>
                <div>
                    <EmailLoginButton/>
                    {/*<GmailLoginButton />*/}
                </div>
            </div>
        </div>
    );
}