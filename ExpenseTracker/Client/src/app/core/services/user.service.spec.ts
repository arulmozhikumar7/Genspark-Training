import { TestBed } from '@angular/core/testing';
import { UserService } from './user.service';

describe('UserService', () => {
  let service: UserService;
  const STORAGE_KEY = 'Guest';

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserService);
    localStorage.clear(); 
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should set and get username from localStorage', () => {
    const name = 'Arul';
    service.setUsername(name);
    expect(localStorage.getItem(STORAGE_KEY)).toBe(name);
    expect(service.getUsername()).toBe(name);
  });

  it('should remove username from localStorage', () => {
    service.setUsername('GuestUser');
    service.removeUsername();
    expect(localStorage.getItem(STORAGE_KEY)).toBeNull();
    expect(service.getUsername()).toBeNull();
  });
});
