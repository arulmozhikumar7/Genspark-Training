import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as AuthActions from './auth.actions';
import { AuthApiService } from '@features/auth/services/auth-api.service';
import { TokenService } from '@core/services/token.service';
import { TourService } from '@core/services/tour.service'; 
import { Router } from '@angular/router';
import {
  catchError,
  map,
  of,
  switchMap,
  tap,
} from 'rxjs';
import { NotificationService } from '@core/services/notification.service';
import { UserService } from '@core/services/user.service';

@Injectable()
export class AuthEffects {
  private actions$ = inject(Actions);
  private authApi = inject(AuthApiService);
  private tokenService = inject(TokenService);
  private userService = inject(UserService);
  private router = inject(Router);
  private toast = inject(NotificationService);
  private tourService = inject(TourService);

  // LOGIN

login$ = createEffect(() =>
  this.actions$.pipe(
    ofType(AuthActions.login),
    switchMap(({ payload }) =>
      this.authApi.login(payload).pipe(
        tap((res) => {
          this.tokenService.setAccessToken(res.data.token);
          this.tokenService.setRefreshToken(res.data.refreshToken);
          this.userService.setUsername(res.data.userName);
        }),
        map((res) => AuthActions.loadTourProgressAfterLogin({ response: res })),
        catchError((err) => {
          this.toast.error(err.error?.message || 'Login failed');
          return of(AuthActions.loginFailure({ error: err.error?.message || 'Login failed' }));
        })
      )
    )
  )
);

loadTourProgress$ = createEffect(() =>
  this.actions$.pipe(
    ofType(AuthActions.loadTourProgressAfterLogin),
    switchMap(({ response }) =>
      this.tourService.loadProgressFromApi().pipe(
        map(() => AuthActions.loginSuccess({ response })),
        catchError(() => {
          return of(AuthActions.loginSuccess({ response })); 
        })
      )
    )
  )
);

  // LOGIN SUCCESS 
  loginRedirect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginSuccess),
        tap(() => {
          this.toast.success('Login successful!');
          this.router.navigate(['/home']);
        })
      ),
    { dispatch: false }
  );

  // REGISTER
  register$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.register),
      switchMap(({ payload }) =>
        this.authApi.register(payload).pipe(
          map(() => {
            this.toast.success('Registration successful!');
            return AuthActions.registerSuccess();
          }),
          catchError((err) => {
            this.toast.error(err.error?.message || 'Registration failed');
            return of(
              AuthActions.registerFailure({
                error: err.error?.message || 'Registration failed',
              })
            );
          })
        )
      )
    )
  );

  // REGISTER SUCCESS 
  registerRedirect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.registerSuccess),
        tap(() => this.router.navigate(['/auth/login']))
      ),
    { dispatch: false }
  );

  // LOGOUT 
  logout$ = createEffect(() =>
  this.actions$.pipe(
    ofType(AuthActions.logout),
    switchMap(() => {
      const token = this.tokenService.getAccessToken() ?? '';
      const refreshToken = this.tokenService.getRefreshToken() ?? '';

      return this.authApi.logout({ token, refreshToken }).pipe(
        tap(() => {
          this.tokenService.clear();
          localStorage.removeItem('budget_alerts');
           this.tourService.clearProgress();
          this.toast.info('Logged out.');
          this.router.navigate(['/auth/login']);
        }),
        catchError(() => {
          this.tokenService.clear();
           localStorage.removeItem('budget_alerts');
           this.userService.removeUsername();
            this.tourService.clearProgress();
          this.toast.warn('Session expired. Logging out.');
          this.router.navigate(['/auth/login']);
          return of(); 
        })
      );
    })
  ),
  { dispatch: false }
);

}
