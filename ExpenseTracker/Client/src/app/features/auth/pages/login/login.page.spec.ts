import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginPage } from './login.page';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { login } from '@store/auth/auth.actions';
import { of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

describe('LoginPage', () => {
  let component: LoginPage;
  let fixture: ComponentFixture<LoginPage>;
  let store: MockStore;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginPage, ReactiveFormsModule],
      providers: [
        provideMockStore({
          initialState: {
            auth: { loading: false, error: null },
          },
        }),{
          provide: ActivatedRoute,
          useValue: {
            params: of({}),
            snapshot: { paramMap: new Map() },
          },
        }
      ],
    }).compileComponents();

    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(LoginPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should have invalid form when empty', () => {
    expect(component.loginForm.invalid).toBeTrue();
  });

  it('should have valid form with correct input', () => {
    component.loginForm.setValue({
      email: 'test@example.com',
      password: 'Password123!',
    });

    expect(component.loginForm.valid).toBeTrue();
  });

  it('should not dispatch login action if form is invalid', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.loginForm.setValue({
      email: '',
      password: '',
    });

    component.onSubmit();

    expect(dispatchSpy).not.toHaveBeenCalled();
  });

  it('should dispatch login action if form is valid', () => {
    const dispatchSpy = spyOn(store, 'dispatch');

    const validCredentials = {
      email: 'test@example.com',
      password: 'Password123!',
    };

    component.loginForm.setValue(validCredentials);
    component.onSubmit();

    expect(dispatchSpy).toHaveBeenCalledWith(login({ payload: validCredentials }));
  });
});
