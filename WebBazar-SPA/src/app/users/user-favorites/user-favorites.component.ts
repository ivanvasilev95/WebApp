import { Component, OnInit } from '@angular/core';
import { Ad } from 'src/app/_models/ad';
import { AdService } from 'src/app/_services/ad.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { LikeService } from 'src/app/_services/like.service';

@Component({
  selector: 'app-user-favorites',
  templateUrl: './user-favorites.component.html',
  styleUrls: ['./user-favorites.component.css']
})
export class UserFavoritesComponent implements OnInit {
  ads: Ad[];

  constructor(private adService: AdService, private likeService: LikeService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadUserLikedAds();
  }

  loadUserLikedAds() {
    this.adService.getUserLikedAds().subscribe((ads: Ad[]) => {
      this.ads = ads;
    }, error => {
      this.alertify.error(error);
    });
  }

  removeAdFromLiked(adId: number) {
    this.likeService.removeAdFromLiked(adId).subscribe(() => {
      this.ads.splice(this.ads.findIndex(a => a.id === adId), 1);
      this.alertify.success('Обявата беше премахната от Наблюдавани');
    }, error => {
      this.alertify.error(error);
    });
  }
}
