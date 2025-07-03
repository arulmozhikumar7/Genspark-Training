// src/app/core/interceptors/auth.interceptor.ts
import {
  HttpInterceptorFn,
  HttpRequest,
  HttpHandlerFn,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TokenService } from '../services/token.service';
import {
  Observable,
  throwError,
  BehaviorSubject,
  of,
} from 'rxjs';
import {
  catchError,
  switchMap,
  filter,
  take,
  finalize,
} from 'rxjs/operators';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  const tokenService = inject(TokenService);
  const http = inject(HttpClient);

  const token = tokenService.getAccessToken();
  const authReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        const refreshToken = tokenService.getRefreshToken();
        const accessToken = tokenService.getAccessToken();

        if (!refreshToken || !accessToken) {
          tokenService.clear();
          return throwError(() => new Error('No refresh token'));
        }

        if (!isRefreshing) {
          isRefreshing = true;
          refreshTokenSubject.next(null);
            console.log("called");
          return http
            .post<any>('/api/v1/Auth/refresh', {
              token: accessToken,
              refreshToken,
            })
            .pipe(
              switchMap((res) => {
                const newToken = res.data.token;
                const newRefresh = res.data.refreshToken;
                console.log(newToken);
                tokenService.setAccessToken(newToken);
                tokenService.setRefreshToken(newRefresh);
                refreshTokenSubject.next(newToken);

                return next(
                  req.clone({
                    setHeaders: {
                      Authorization: `Bearer ${newToken}`,
                    },
                  })
                );
              }),
              catchError((err) => {
                tokenService.clear();
                return throwError(() => err);
              }),
              finalize(() => {
                isRefreshing = false;
              })
            );
        } else {
          return refreshTokenSubject.pipe(
            filter((t) => t !== null),
            take(1),
            switchMap((newToken) =>
              next(
                req.clone({
                  setHeaders: {
                    Authorization: `Bearer ${newToken}`,
                  },
                })
              )
            )
          );
        }
      }

      return throwError(() => error);
    })
  );
};
