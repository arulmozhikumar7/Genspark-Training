import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private isAuthenticatedSignal = signal(this.hasToken());

  private hasToken(): boolean {
    return !!localStorage.getItem('mock_token');
  }

  login(username: string, password: string): boolean {
    if (username === 'admin' && password === 'admin') {
      localStorage.setItem('mock_token', '123');
      this.isAuthenticatedSignal.set(true);
      return true;
    }
    return false;
  }

  logout(): void {
    localStorage.removeItem('mock_token');
    this.isAuthenticatedSignal.set(false);
  }

  isAuthenticated(): boolean {
    return this.isAuthenticatedSignal();
  }

  authState = this.isAuthenticatedSignal.asReadonly();
}