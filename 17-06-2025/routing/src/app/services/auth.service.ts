import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private _isLoggedIn = signal<boolean>(false);

  isAuthenticated() {
    return this._isLoggedIn();
  }

  login(username: string, password: string): boolean {
    if (username === 'admin' && password === 'admin') {
      this._isLoggedIn.set(true);
      return true;
    }
    return false;
  }

  logout() {
    this._isLoggedIn.set(false);
  }
}
