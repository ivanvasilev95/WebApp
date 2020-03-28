import {Injectable} from '@angular/core';
import {Ad} from '../_models/ad';
import { Resolve, ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AdService } from '../_services/ad.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class AdDetailResolver implements Resolve<Ad> {
    constructor(private adService: AdService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Ad> {
        // tslint:disable-next-line: no-string-literal
        return this.adService.getAd(route.params['id']).pipe(
            catchError(error => {
                this.alertify.error(error); // 'Проблем с получаването на данните'
                this.router.navigate(['/ads']);
                return of(null);
            })
        );
    }
}