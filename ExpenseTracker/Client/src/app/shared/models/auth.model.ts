export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  userName: string;
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiresIn?: number;
  user: AuthUser;
}

export interface AuthUser {
  id: string;
  userName: string;
  email: string;
  role?: string;
}

export interface RefreshTokenRequest {
  token: string;
  refreshToken: string;
}
