import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SidebarComponent } from './sidebar.component';
import { Store } from '@ngrx/store';
import { UserService } from '@core/services/user.service';
import { TokenService } from '@core/services/token.service';
import { Renderer2 } from '@angular/core';
import { logout } from '@store/auth/auth.actions';
import { of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

class Renderer2Mock {
  addClass = jasmine.createSpy('addClass').and.callFake((el: any, className: string) => {
    el.classList.add(className);
  });

  removeClass = jasmine.createSpy('removeClass').and.callFake((el: any, className: string) => {
    el.classList.remove(className);
  });
}

describe('SidebarComponent', () => {
  let fixture: ComponentFixture<SidebarComponent>;
  let component: SidebarComponent;

  let storeSpy: jasmine.SpyObj<Store>;
  let userServiceSpy: jasmine.SpyObj<UserService>;
  let tokenServiceSpy: jasmine.SpyObj<TokenService>;
  let rendererMock: Renderer2Mock;

  beforeEach(() => {
    storeSpy = jasmine.createSpyObj('Store', ['dispatch']);
    userServiceSpy = jasmine.createSpyObj('UserService', ['getUsername']);
    tokenServiceSpy = jasmine.createSpyObj('TokenService', ['getUserFromToken']);
    rendererMock = new Renderer2Mock();

    TestBed.configureTestingModule({
      imports: [SidebarComponent],
      providers: [
        { provide: Store, useValue: storeSpy },
        { provide: UserService, useValue: userServiceSpy },
        { provide: TokenService, useValue: tokenServiceSpy },
        { provide: Renderer2, useValue: rendererMock },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({}),
            snapshot: { paramMap: new Map() },
          },
        },
      ],
    });

    fixture = TestBed.createComponent(SidebarComponent);
    component = fixture.componentInstance;
  });

  describe('ngOnInit', () => {
    it('should set userName from userService', () => {
      userServiceSpy.getUsername.and.returnValue('TestUser');
      tokenServiceSpy.getUserFromToken.and.returnValue({
        id: '123',
        email: 'test@example.com',
        role: 'User', // not Admin
      });

      document.body.classList.remove('dark');

      fixture.detectChanges();

      expect(component.userName).toBe('TestUser');
      expect(component.isDarkMode).toBeFalse();
      expect(component.isAdmin).toBeFalse();
    });

    it('should set isAdmin true if role is Admin', () => {
      userServiceSpy.getUsername.and.returnValue('AdminUser');
      tokenServiceSpy.getUserFromToken.and.returnValue({
        id: '123',
        email: 'test@example.com',
        role: 'Admin',
      });

      document.body.classList.add('dark');

      fixture.detectChanges();

      expect(component.userName).toBe('AdminUser');
      expect(component.isDarkMode).toBeTrue();
      expect(component.isAdmin).toBeTrue();
    });

  
  });

  describe('onLogout', () => {
    it('should dispatch logout action', () => {
      component.onLogout();
      expect(storeSpy.dispatch).toHaveBeenCalledWith(logout());
    });
  });

 
});
