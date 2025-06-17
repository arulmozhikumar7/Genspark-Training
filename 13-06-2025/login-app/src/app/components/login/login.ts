import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit {
  username = '';
  password = '';
  error = '';

  constructor(private auth: AuthService, private router: Router) {}

  ngOnInit(): void {
    const existingUser = this.auth.getstoredUser();
    if (existingUser) {
      this.router.navigate(['/dashboard']);
    }
  }

  login(): void {
    const user = { username: this.username, password: this.password };
    if (this.auth.login(user)) {
      this.auth.storeUser(user);
      this.router.navigate(['/dashboard']);
    } else {
      this.error = 'Invalid credentials';
    }
  }
}
