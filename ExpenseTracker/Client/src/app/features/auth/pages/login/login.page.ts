import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { login } from '@store/auth/auth.actions';
import { selectAuthError, selectAuthLoading } from '@store/auth/auth.selectors';
import { LoginRequest } from '@shared/models/auth.model';
import { AsyncPipe } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-login-page',
  imports: [CommonModule, ReactiveFormsModule, AsyncPipe,RouterModule],
  templateUrl: './login.page.html',
})
export class LoginPage {
  private fb = inject(FormBuilder);
  private store = inject(Store);

  loading$ = this.store.select(selectAuthLoading);
  error$ = this.store.select(selectAuthError);

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  get f() {
    return this.loginForm.controls;
  }

  onSubmit(): void {
    if (this.loginForm.invalid) return;

    const payload: LoginRequest = this.loginForm.value as LoginRequest;
    this.store.dispatch(login({ payload }));
  }
}
