import { Injectable } from "@angular/core";
import { User } from "../models/user.model";

@Injectable({
    providedIn: 'root'
})

export class AuthService{
    private users: User[] = [
        { username: 'admin', password: 'qwerty123' },
        { username: 'user' , password: 'qwerty123' }
    ]

    login(user:User):boolean{
        return this.users.some(u => u.username === user.username && u.password === user.password);
    }

    storeUser(user:User): void{
        localStorage.setItem('loggedInUser',JSON.stringify(user));
    }

    getstoredUser(): User| null {
        const storeduser = localStorage.getItem('loggedInUser');
        return storeduser ? JSON.parse(storeduser) : null;
    }

    logout(): void{
        localStorage.removeItem('loggedInUser');
    }
    
}