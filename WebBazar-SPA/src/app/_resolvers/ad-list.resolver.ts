import {Injectable} from '@angular/core';
import {Ad} from '../_models/ad';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AdService } from '../_services/ad.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class AdListResolver implements Resolve<Ad[]> {
    pageNumber = 1;
    pageSize = 6;

    constructor(private adService: AdService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Ad[]> {
        return this.adService.getAds(this.pageNumber, this.pageSize).pipe(
            catchError(error => {
                this.alertify.error(error);
                this.router.navigate(['']);
                return of(null);
            })
        );
    }
}
