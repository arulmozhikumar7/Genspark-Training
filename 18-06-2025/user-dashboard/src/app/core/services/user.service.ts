import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { User } from '../models/user.model';
@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly baseUrl = 'https://dummyjson.com/users/';

  addUser(user: Partial<User>): Observable<User> {
    return from(fetch(this.baseUrl+`add`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(user)
    })).pipe(
      switchMap(res => res.json())
    );
  }

  getUsers(): Observable<User[]> {
    return from(fetch(this.baseUrl)).pipe(
      switchMap(res => res.json()),
      map(data => data.users)
    );
  }
}
