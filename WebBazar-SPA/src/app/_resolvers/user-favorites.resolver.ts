import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Ad } from '../_models/ad';
import { AdService } from '../_services/ad.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable()
export class UserFavoritesResolver implements Resolve<Ad[]> {

  constructor(private adService: AdService, private router: Router, private alertify: AlertifyService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<Ad[]> {
    return this.adService.getUserLikedAds().pipe(
      catchError(error => {
        this.alertify.error(error);
        this.router.navigate(['']);
        return of(null);
      })
    );
  }
}
