import { Component, OnInit } from '@angular/core';
import { Ad } from '../_models/ad';
import { AdService } from '../_services/ad.service';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-favorites',
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.css']
})
export class FavoritesComponent implements OnInit {
  ads: Ad[];

  constructor(private adService: AdService, private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadUserFavorites();
  }

  loadUserFavorites() {
    this.adService.getUserFavorites(this.authService.decodedToken.nameid).subscribe((ads: Ad[]) => {
      this.ads = ads;
    }, error => {
      this.alertify.error(error);
    });
  }

  removeAd(adId: number) {
    this.adService.removeAd(this.authService.decodedToken.nameid, adId).subscribe(res => {
      this.loadUserFavorites();
      this.alertify.success('Ad removed successfully from Favorites');
    }, error => {
      this.alertify.error(error);
    });
  }
}
