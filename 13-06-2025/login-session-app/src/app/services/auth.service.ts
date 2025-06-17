import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private users = [
    { username: 'admin', password: 'qwerty123' },
    { username: 'user', password: 'qwerty123' }
  ];

  login(user: { username: string; password: string }): boolean {
    return this.users.some(u => u.username === user.username && u.password === user.password);
  }

  storeUser(user: { username: string; password: string }): void {
    sessionStorage.setItem('user', JSON.stringify(user));
  }

  getStoredUser(): { username: string; password: string } | null {
    const json = sessionStorage.getItem('user');
    return json ? JSON.parse(json) : null;
  }

  logout(): void {
    sessionStorage.removeItem('user');
  }
}
