import { createAction, props } from '@ngrx/store';
import {
  LoginRequest,
  RegisterRequest,
  AuthResponse,
} from '@shared/models/auth.model';

// Login Actions
export const login = createAction(
  '[Auth] Login',
  props<{ payload: LoginRequest }>()
);

export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ response: AuthResponse }>()
);

export const loginFailure = createAction(
  '[Auth] Login Failure',
  props<{ error: string }>()
);

export const loadTourProgressAfterLogin = createAction(
  '[Auth] Load Tour Progress After Login',
  props<{ response: any }>()
);

// Register Actions
export const register = createAction(
  '[Auth] Register',
  props<{ payload: RegisterRequest }>()
);

export const registerSuccess = createAction('[Auth] Register Success');

export const registerFailure = createAction(
  '[Auth] Register Failure',
  props<{ error: string }>()
);

// Logout Action
export const logout = createAction('[Auth] Logout');
