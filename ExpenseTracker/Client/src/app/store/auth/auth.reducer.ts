import { createReducer, on } from '@ngrx/store';
import * as AuthActions from './auth.actions';
import { AuthState, initialAuthState } from './auth.state';

export const authReducer = createReducer(
  initialAuthState,

  // Login
  on(AuthActions.login, (state): AuthState => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(AuthActions.loginSuccess, (state, { response }): AuthState => ({
    ...state,
    loading: false,
    user: response.user,
    token: response.token,
    refreshToken: response.refreshToken,
    error: null,
  })),

  on(AuthActions.loginFailure, (state, { error }): AuthState => ({
    ...state,
    loading: false,
    error,
  })),

  // Register
  on(AuthActions.register, (state): AuthState => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(AuthActions.registerSuccess, (state): AuthState => ({
    ...state,
    loading: false,
    error: null,
  })),

  on(AuthActions.registerFailure, (state, { error }): AuthState => ({
    ...state,
    loading: false,
    error,
  })),

  // Logout
  on(AuthActions.logout, (): AuthState => ({
    ...initialAuthState,
  }))
);
