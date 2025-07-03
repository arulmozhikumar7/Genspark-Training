import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, Validators, FormBuilder, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { Store } from '@ngrx/store';
import { RouterModule } from '@angular/router';
import { AsyncPipe } from '@angular/common';

import { register } from '@store/auth/auth.actions';
import { selectAuthError, selectAuthLoading } from '@store/auth/auth.selectors';
import { RegisterRequest } from '@shared/models/auth.model';

@Component({
  standalone: true,
  selector: 'app-register-page',
  imports: [CommonModule, ReactiveFormsModule, AsyncPipe, RouterModule],
  templateUrl: './register.page.html',
})
export class RegisterPage {
  private fb = inject(FormBuilder);
  private store = inject(Store);
   private passwordMatchValidator: ValidatorFn = (
    group: AbstractControl
  ): ValidationErrors | null => {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  };

  loading$ = this.store.select(selectAuthLoading);
  error$ = this.store.select(selectAuthError);

  registerForm = this.fb.nonNullable.group(
    {
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/
          ),
        ],
      ],
      confirmPassword: [''],
    },
    { validators: this.passwordMatchValidator }
  );

  get f() {
    return this.registerForm.controls;
  }

 
  onSubmit(): void {
    if (this.registerForm.invalid) return;

    const { userName, email, password } = this.registerForm.getRawValue();

    const payload: RegisterRequest = {
      userName,
      email,
      password,
    };

    this.store.dispatch(register({ payload }));
  }
}
