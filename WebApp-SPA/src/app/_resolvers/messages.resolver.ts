import {Injectable} from '@angular/core';
import {Ad} from '../_models/ad';
import { Resolve, ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AdService } from '../_services/ad.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../_models/message';
import { UserService } from '../_services/user.service';

@Injectable()
export class MessagesResolver implements Resolve<Message[]> {
    pageNumber = 1;
    pageSize = 5;
    messageContainer = 'Unread';

    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
        // tslint:disable-next-line: no-string-literal
        return this.userService.getMessages(this.pageNumber, this.pageSize, this.messageContainer).pipe(
            catchError(error => {
                this.alertify.error(error); // 'Проблем с получаването на данните'
                this.router.navigate(['']);
                return of(null);
            })
        );
    }
}
