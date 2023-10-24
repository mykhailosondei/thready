import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.checkForActivation(state);
  }



  private checkForActivation(state: RouterStateSnapshot) {
    const potentialAccessToken = localStorage.getItem("token");
    if (potentialAccessToken == null) {
      this.router.navigateByUrl('/login');
      return false;
    }
    return true;
  }
}
