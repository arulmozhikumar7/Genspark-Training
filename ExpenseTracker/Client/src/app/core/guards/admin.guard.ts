import { inject } from '@angular/core';
import {
  CanActivateFn,
  Router,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
} from '@angular/router';

import { TokenService } from '@core/services/token.service';

export const adminGuardFn: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const tokenService = inject(TokenService);
  const router = inject(Router);

  const token = tokenService.getAccessToken();
  if (!token) {
    router.navigate(['/auth/login']);
    return false;
  }

  try {
    const decoded: any = tokenService.getUserFromToken();

    const roleClaim = decoded?.role || decoded?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    if (roleClaim === 'Admin') {
      return true;
    } else {
      // Not an admin — redirect to home or 403 page
      router.navigate(['/home']);
      return false;
    }
  } catch (error) {
    console.error('Error decoding token:', error);
    router.navigate(['/auth/login']);
    return false;
  }
};
