import { Injectable } from '@angular/core';
import { HttpService } from '@core/services/http.service';
import { LoginRequest, RegisterRequest } from '@shared/models/auth.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  constructor(private http: HttpService) {}

  login(payload: LoginRequest): Observable<any> {
    return this.http.post('/Auth/login', payload);
  }

  register(payload: RegisterRequest): Observable<any> {
    return this.http.post('/User/register', payload);
  }

  refreshToken(payload: { token: string; refreshToken: string }): Observable<any> {
    return this.http.post('/Auth/refresh', payload);
  }

  logout(payload: { token: string; refreshToken: string }): Observable<any> {
    return this.http.post('/Auth/logout', payload);
  }
}
