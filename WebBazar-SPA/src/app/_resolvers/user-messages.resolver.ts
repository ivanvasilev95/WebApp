import {Injectable} from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../_models/message';
import { MessageService } from '../_services/message.service';

@Injectable()
export class UserMessagesResolver implements Resolve<Message[]> {
    pageNumber = 1;
    pageSize = 5;
    messageFilter = 'Unread';

    constructor(private messageService: MessageService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
        return this.messageService.getMessages(this.pageNumber, this.pageSize, this.messageFilter).pipe(
            catchError(error => {
                this.alertify.error(error);
                this.router.navigate(['']);
                return of(null);
            })
        );
    }
}
