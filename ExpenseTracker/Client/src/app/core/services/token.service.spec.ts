import { TestBed } from '@angular/core/testing';
import { TokenService } from './token.service';

describe('TokenService', () => {
  let service: TokenService;

  const mockAccessToken = 'mockedAccessToken';
  const mockRefreshToken = 'mockedRefreshToken';

  const mockDecodedToken = {
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier': '123',
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress': 'test@example.com',
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': 'Admin',
  };

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TokenService);
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should set and get access token', () => {
    service.setAccessToken(mockAccessToken);
    expect(service.getAccessToken()).toBe(mockAccessToken);
  });

  it('should remove access token', () => {
    service.setAccessToken(mockAccessToken);
    service.removeAccessToken();
    expect(service.getAccessToken()).toBeNull();
  });

  it('should set and get refresh token', () => {
    service.setRefreshToken(mockRefreshToken);
    expect(service.getRefreshToken()).toBe(mockRefreshToken);
  });

  it('should remove refresh token', () => {
    service.setRefreshToken(mockRefreshToken);
    service.removeRefreshToken();
    expect(service.getRefreshToken()).toBeNull();
  });

  it('should clear both tokens', () => {
    service.setAccessToken(mockAccessToken);
    service.setRefreshToken(mockRefreshToken);
    service.clear();
    expect(service.getAccessToken()).toBeNull();
    expect(service.getRefreshToken()).toBeNull();
  });


  it('should return null if no token is present', () => {
    const user = service.getUserFromToken();
    expect(user).toBeNull();
  });

});
