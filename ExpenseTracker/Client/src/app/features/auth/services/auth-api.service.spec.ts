import { TestBed } from '@angular/core/testing';
import { AuthApiService } from './auth-api.service';
import { HttpService } from '@core/services/http.service';
import { of } from 'rxjs';
import { LoginRequest, RegisterRequest } from '@shared/models/auth.model';

describe('AuthApiService', () => {
  let service: AuthApiService;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;

  beforeEach(() => {
    httpServiceSpy = jasmine.createSpyObj('HttpService', ['post']);

    TestBed.configureTestingModule({
      providers: [
        AuthApiService,
        { provide: HttpService, useValue: httpServiceSpy },
      ],
    });

    service = TestBed.inject(AuthApiService);
  });

  it('should call http.post with correct params on login', () => {
    const mockResponse = { token: 'abc' };
    httpServiceSpy.post.and.returnValue(of(mockResponse));

    const payload: LoginRequest = { email: 'test@example.com', password: '123456' };

    service.login(payload).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    expect(httpServiceSpy.post).toHaveBeenCalledWith('/Auth/login', payload);
  });

  it('should call http.post with correct params on register', () => {
    const mockResponse = { userId: '123' };
    httpServiceSpy.post.and.returnValue(of(mockResponse));

    const payload: RegisterRequest = { email: 'test@example.com', password: '123456',userName:'demo' };

    service.register(payload).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    expect(httpServiceSpy.post).toHaveBeenCalledWith('/User/register', payload);
  });

  it('should call http.post with correct params on refreshToken', () => {
    const mockResponse = { token: 'newtoken' };
    httpServiceSpy.post.and.returnValue(of(mockResponse));

    const payload = { token: 'oldtoken', refreshToken: 'oldrefresh' };

    service.refreshToken(payload).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    expect(httpServiceSpy.post).toHaveBeenCalledWith('/Auth/refresh', payload);
  });

  it('should call http.post with correct params on logout', () => {
    const mockResponse = {};
    httpServiceSpy.post.and.returnValue(of(mockResponse));

    const payload = { token: 'token', refreshToken: 'refresh' };

    service.logout(payload).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    expect(httpServiceSpy.post).toHaveBeenCalledWith('/Auth/logout', payload);
  });
});
