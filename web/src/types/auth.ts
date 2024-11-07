export interface User {
    ID: string;
    Email: string;
    Name: string;
    Picture?: string;
}

export interface Session {
    picture: string;
    email: string;
    name: string;
    user_id: string;
    isLoggedIn: boolean;
}