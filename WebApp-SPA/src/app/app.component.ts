import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from './_services/auth.service';
import { MessageService } from './_services/message.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(private authService: AuthService, private messageService: MessageService) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (token) {
      const jwtHelper = new JwtHelperService();
      this.authService.decodedToken = jwtHelper.decodeToken(token);
      this.messageService.getUnreadMessagesCount().subscribe();
    }
  }
}
