export interface User{
    username : string;
    email: string;
    password: string;
    role: 'Admin' | 'User' | 'Guest';
}