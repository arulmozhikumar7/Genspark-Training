import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RoleRedirectComponent } from './role-redirect.component';
import { Router } from '@angular/router';
import { TokenService } from '@core/services/token.service';

describe('RoleRedirectComponent', () => {
  let fixture: ComponentFixture<RoleRedirectComponent>;
  let routerSpy: jasmine.SpyObj<Router>;
  let tokenServiceSpy: jasmine.SpyObj<TokenService>;

  beforeEach(() => {
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    tokenServiceSpy = jasmine.createSpyObj('TokenService', ['getUserFromToken']);

    TestBed.configureTestingModule({
      imports: [RoleRedirectComponent],  // <-- standalone component imported here
      providers: [
        { provide: Router, useValue: routerSpy },
        { provide: TokenService, useValue: tokenServiceSpy }
      ]
    });

    fixture = TestBed.createComponent(RoleRedirectComponent);
  });

  it('should redirect to /category for Admin role', () => {
    tokenServiceSpy.getUserFromToken.and.returnValue({
      id: '1',
      email: 'admin@example.com',
      role: 'Admin',
    });
    fixture.detectChanges(); // triggers ngOnInit
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/category']);
  });

  it('should redirect to /home for User role', () => {
    tokenServiceSpy.getUserFromToken.and.returnValue({
      id: '2',
      email: 'user@example.com',
      role: 'User',
    });
    fixture.detectChanges();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/home']);
  });

  it('should redirect to /auth/login for unknown role', () => {
    tokenServiceSpy.getUserFromToken.and.returnValue({
      id: '3',
      email: 'other@example.com',
      role: 'Other',
    });
    fixture.detectChanges();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/auth/login']);
  });

  it('should redirect to /auth/login if no user data', () => {
    tokenServiceSpy.getUserFromToken.and.returnValue(null);
    fixture.detectChanges();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/auth/login']);
  });
});
