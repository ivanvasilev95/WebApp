import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Ad } from '../_models/ad';
import { Observable, of } from 'rxjs';
import { AlertifyService } from '../_services/alertify.service';
import { AdService } from '../_services/ad.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class AdEditResolver implements Resolve<Ad> {
    constructor(private adService: AdService,
                private alertify: AlertifyService,
                private router: Router) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Ad> {
        return this.adService.getAd(route.params.id).pipe(
            catchError(error => {
                this.alertify.error(error);
                this.router.navigate(['user/ads']);
                return of(null);
            })
        );
    }
}
