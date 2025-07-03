import {  inject } from '@angular/core';
import {
  CanActivateFn,
  Router,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
} from '@angular/router';

import { TokenService } from '@core/services/token.service';

export const authGuardFn: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const tokenService = inject(TokenService);
  const token = tokenService.getAccessToken();

  if (token) {
    return true;
  } else {
    const router = inject(Router);
    router.navigate(['/auth/login']);
    return false;
  }
};