import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseURL = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient, private userService: UserService) { }

  login(model: any) {
    return this.http.post(this.baseURL + 'login', model)
      .pipe(
        map((response: any) => {
          const user = response;
          if (user) {
            localStorage.setItem('token', user.token);
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
          }

          this.http.get(environment.apiUrl + 'messages/user/unread').subscribe((count: number) => {
            this.userService.unreadMessagesCount = count;
          });
        })
      );
  }

  register(user: User) {
    return this.http.post(this.baseURL + 'register', user);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  roleMatch(allowedRoles): boolean {
    let isMatch = false;
    const userRoles = this.decodedToken.role as Array<string>;
    if (userRoles) {
      allowedRoles.forEach(element => {
        if (userRoles.includes(element)) {
          isMatch = true;
          return;
        }
      });
    }
    return isMatch;
  }
}
