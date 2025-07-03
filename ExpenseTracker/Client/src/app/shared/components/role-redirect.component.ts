
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '@core/services/token.service';

@Component({
  selector: 'app-role-redirect',
  template: '',
})
export class RoleRedirectComponent {
  private router = inject(Router);
  private tokenService = inject(TokenService);

ngOnInit() {
  const data = this.tokenService.getUserFromToken();

  if (data?.role === 'Admin') {
    this.router.navigate(['/category']);
  } else if (data?.role === 'User') {
    this.router.navigate(['/home']);
  } else {
    this.router.navigate(['/auth/login']); 
  }
}

}
