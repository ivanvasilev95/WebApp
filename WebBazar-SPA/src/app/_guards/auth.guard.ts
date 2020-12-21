import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private alertify: AlertifyService) {}

  canActivate(next: ActivatedRouteSnapshot): boolean {
    const roles = next.firstChild.data.roles as Array<string>;
    if (roles) {
      if (!this.authService.loggedIn()) {
        this.alertify.error('Нямате право на достъп до тази страница');
        this.router.navigate(['/auth']);
        return false;
      }
	  
      const match = this.authService.roleMatch(roles);
      if (match) {
        return true;
      } else {
        this.alertify.error('Нямате право на достъп до тази страница');
        this.router.navigate(['']);
        return false;
      }
    }

    if (this.authService.loggedIn()) {
      return true;
    }

    this.alertify.error('Нямате право на достъп до тази страница');
    this.router.navigate(['/auth']);
    return false;
  }
}
