import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthenticationService, 
    private router: Router) { }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot){
    await this.authService.loadTokensFromStorage();
    const token = this.authService.token;
    if (token){
      return true;
    }
    this.router.navigate(['auth'], { queryParams: { returnUrl: state.url }});
    return false;
  }
}
