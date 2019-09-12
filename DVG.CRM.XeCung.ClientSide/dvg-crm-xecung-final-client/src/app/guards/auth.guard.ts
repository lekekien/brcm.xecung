import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private cookie: CookieService
  ) { }
  canActivate( route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const currentUser = this.authenticationService.currentUserValue;
    if (currentUser) {
        // check if route is restricted by role
        if (route.data.roles && route.data.roles.filter(value => -1 !== currentUser.Roles.indexOf(value)).length === 0) {
            // role not authorised so redirect to home page
            this.router.navigate(['/']);
            return false;
      }

        // authorised so return true
        return true;
    }
    // not logged in so redirect to login page with the return url
    this.cookie.delete('UserToken');
    localStorage.removeItem('currentUser');
    localStorage.removeItem('Location');
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
