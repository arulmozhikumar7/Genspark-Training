import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  template: `
    <h2>Login</h2>
    <form (ngSubmit)="login()">
      <label>Username: <input [(ngModel)]="username" name="username" /></label><br>
      <label>Password: <input [(ngModel)]="password" name="password" type="password" /></label><br>
      <button type="submit">Login</button>
      <p *ngIf="error" style="color:red">{{ error }}</p>
    </form>
  `
})
export class LoginComponent {
  username = '';
  password = '';
  error = '';

  constructor(private auth: AuthService, private router: Router) {}

  login() {
    const success = this.auth.login(this.username, this.password);
    if (success) {
      this.router.navigate(['/admin']);
    } else {
      this.error = 'Invalid credentials';
    }
  }
}
