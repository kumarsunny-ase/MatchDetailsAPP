import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from '../services/auth.service';
import jwt_decode, { jwtDecode } from 'jwt-decode';

export const authGuard: CanActivateFn = (route, state) => {
  const cookieService = inject(CookieService);
  const router = inject(Router);
  const authService = inject(AuthService);
  const user = authService.getUser();

  // check the JWT Token
  let token = cookieService.get('Authorization');

  if(token && user) {
    token = token.replace('Bearer ', '');
    const decodedToken: any = jwtDecode(token);

    // Check if tokken has expired
    const expirationDate = decodedToken.exp * 1000;
    const currentTime = new Date().getTime();
    

    if(expirationDate < currentTime ) {
      // Logout
      authService.logout();
      return router.createUrlTree([''], {
        queryParams: { returnUrl: state.url },
      })
    } else {
      // Token is still valid

      if(user.roles.includes('User')) {
        return true;
      } else {
        alert('Unauthorized');
        return false;
      }
    }
  } else {
    // Logout
    authService.logout();
    return router.createUrlTree([''], {queryParams: { returnUrl: state.url }})
  }
};
