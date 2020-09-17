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
    this.loadUserLikedAds();
  }

  loadUserLikedAds() {
    this.adService.getUserLikedAds(this.authService.decodedToken.nameid).subscribe((ads: Ad[]) => {
      this.ads = ads;
    }, error => {
      this.alertify.error(error);
    });
  }

  removeAdFromLiked(adId: number) {
    this.adService.removeAdFromLiked(this.authService.decodedToken.nameid, adId).subscribe(res => {
      this.ads.splice(this.ads.findIndex(a => a.id === adId), 1);
      this.alertify.success('Обявата беше премахната от Наблюдавани');
    }, error => {
      this.alertify.error(error);
    });
  }
}
