import {Injectable} from '@angular/core';
import {Ad} from '../_models/ad';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AdService } from '../_services/ad.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class AdDetailResolver implements Resolve<Ad> {
    constructor(private adService: AdService, private authService: AuthService,
                private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Ad> {
        return this.adService.getAd(route.params.id).pipe(
            tap ((ad: Ad) => {
                if (!ad.isApproved && (!this.userIsLoggedIn() || (this.userIsLoggedIn() && this.userDoesntHaveRights(ad.userId)))) {
                    this.returnUserToAds('Обявата не е намерена');
                }
            }),
            catchError(error => {
                this.returnUserToAds(error);
                return of(null);
            })
        );
    }

    returnUserToAds(message) {
        this.alertify.error(message);
        this.router.navigate(['/ads']);
    }

    userDoesntHaveRights(adOwnerId: number) {
        // not the ad owner and not admin or moderator
        return adOwnerId !== this.getLoggedInUserId() && !this.authService.roleMatch(['Admin', 'Moderator']);
    }

    userIsLoggedIn() {
        return this.authService.loggedIn();
    }

    getLoggedInUserId() {
        return +this.authService.decodedToken.nameid;
    }
}
