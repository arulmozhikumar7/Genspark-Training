import { Injectable } from "@angular/core";
import { BehaviorSubject,Observable,of } from "rxjs";
import { map } from "rxjs/operators";
import { User } from "../models/user.model";

@Injectable({
    providedIn: 'root'
})
export class UserService{
    private usersSubject = new BehaviorSubject<User[]>([]);
    private users$ = this.usersSubject.asObservable();

    constructor(){
        const initialUsers: User[] = [
            { username: 'cristiano' , email: 'cristiano@gmail.com', password: '', role: 'Admin'},
            { username: 'kroos' , email: 'kroos@gmail.com', password: '', role: 'User'},
            { username: 'modric' , email: 'modric@gmail.com', password: '', role: 'Guest'}
        ];
        this.usersSubject.next(initialUsers);
    }

    addUser(user: User): void{
        const currentUsers = this.usersSubject.getValue();
        this.usersSubject.next([...currentUsers,user]);
    }

    getUsers(): Observable<User[]>{
        return this.users$;
    }

    searchUsers(query: string, roleFilter?: string):Observable<User[]>{
        const lowerQuery = query.toLowerCase();

        return this.users$.pipe(
            map(users=>
                users.filter(user=>{
                    const matchesUsername = user.username.toLowerCase().includes(lowerQuery);
                    const matchesRole = roleFilter ? user.role === roleFilter : true;
                    return matchesUsername && matchesRole ;
                }
                )
            )
        )
    }
}