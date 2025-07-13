import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegisterPage } from './register.page';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { register } from '@store/auth/auth.actions';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

describe('RegisterPage', () => {
  let component: RegisterPage;
  let fixture: ComponentFixture<RegisterPage>;
  let store: MockStore;
  const initialState = {
    auth: {
      loading: false,
      error: null,
    },
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterPage, ReactiveFormsModule],
      providers: [provideMockStore({ initialState }), {
          provide: ActivatedRoute,
          useValue: {
            params: of({}),
            snapshot: { paramMap: new Map() },
          },
        }],
      
    }).compileComponents();

    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(RegisterPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should have an invalid form if fields are empty', () => {
    expect(component.registerForm.invalid).toBeTrue();
  });

  it('should mark form as invalid if passwords do not match', () => {
    component.registerForm.setValue({
      userName: 'testuser',
      email: 'test@example.com',
      password: 'Password123!',
      confirmPassword: 'Different123!',
    });

    expect(component.registerForm.invalid).toBeTrue();
    expect(component.registerForm.errors?.['passwordMismatch']).toBeTrue();
  });

  it('should mark form as valid if all fields are correct', () => {
    component.registerForm.setValue({
      userName: 'testuser',
      email: 'test@example.com',
      password: 'Password123!',
      confirmPassword: 'Password123!',
    });

    expect(component.registerForm.valid).toBeTrue();
  });

  it('should dispatch register action on valid form submission', () => {
    const dispatchSpy = spyOn(store, 'dispatch');

    component.registerForm.setValue({
      userName: 'testuser',
      email: 'test@example.com',
      password: 'Password123!',
      confirmPassword: 'Password123!',
    });

    component.onSubmit();

    expect(dispatchSpy).toHaveBeenCalledWith(
      register({
        payload: {
          userName: 'testuser',
          email: 'test@example.com',
          password: 'Password123!',
        },
      })
    );
  });

  it('should NOT dispatch register action if form is invalid', () => {
    const dispatchSpy = spyOn(store, 'dispatch');

    component.registerForm.setValue({
      userName: '',
      email: 'invalid-email',
      password: '123',
      confirmPassword: '456',
    });

    component.onSubmit();

    expect(dispatchSpy).not.toHaveBeenCalled();
  });
});
