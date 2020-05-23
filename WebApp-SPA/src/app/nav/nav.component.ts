import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {}

  loggedIn() {
    // const token = localStorage.getItem('token');
    // return !!token;
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('Успешно излезнахте от системата');
    this.router.navigate(['/home']);
  }

  hasRole(roles) {
    const userRoles = this.authService.decodedToken.role as Array<string>;

    if (!userRoles) {
      return false;
    }

    if (this.authService.roleMatch(roles)) {
      return true;
    }

    return false;
  }
}
