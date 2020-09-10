import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {

  constructor(public authService: AuthService,
              private alertify: AlertifyService,
              private router: Router) { }

  get unreadMessagesCount() {
      return MessageService.unreadMessagesCount;
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('Успешно излезнахте от системата');
    this.router.navigate(['/home']);
  }

  hasRole(roles) {
    return this.authService.roleMatch(roles);
  }
}
