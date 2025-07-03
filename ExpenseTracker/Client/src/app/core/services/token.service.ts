import { Injectable } from '@angular/core';
import {jwtDecode} from 'jwt-decode';

interface DecodedToken {
  [key: string]: any;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'?: string;
}
const ACCESS_TOKEN_KEY = 'access_token';
const REFRESH_TOKEN_KEY = 'refresh_token';

@Injectable({ providedIn: 'root' })
export class TokenService {
  // Access Token
  setAccessToken(token: string): void {
    localStorage.setItem(ACCESS_TOKEN_KEY, token);
  }

  getAccessToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
  }

  removeAccessToken(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
  }

  // Refresh Token
  setRefreshToken(token: string): void {
    localStorage.setItem(REFRESH_TOKEN_KEY, token);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  }

  removeRefreshToken(): void {
    localStorage.removeItem(REFRESH_TOKEN_KEY);
  }

  // Clear all tokens
  clear(): void {
    this.removeAccessToken();
    this.removeRefreshToken();
  }

  getUserFromToken() {
    const token = this.getAccessToken();
    if (!token) return null;

    try {
      const decoded: DecodedToken = jwtDecode(token);
      return {
        id: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
        email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
        role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
      };
    } catch (e) {
      console.error('Invalid token', e);
      return null;
    }
  }

}
