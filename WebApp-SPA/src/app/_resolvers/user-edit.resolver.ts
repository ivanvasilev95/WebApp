import {Injectable} from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';

@Injectable()
export class UserEditResolver implements Resolve<User> {
    constructor(private userService: UserService, private authService: AuthService,
                private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getUser(this.authService.decodedToken.nameid).pipe(
            catchError(error => {
                this.alertify.error(error); // 'Проблем с получаването на данните'
                this.router.navigate(['']);
                return of(null);
            })
        );
    }
}