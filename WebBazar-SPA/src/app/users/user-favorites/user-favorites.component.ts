import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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

  constructor(private likeService: LikeService, private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.ads = data.ads;
    });
  }

  unlikeAd(adId: number) {
    this.likeService.unlikeAd(adId).subscribe(() => {
      this.ads.splice(this.ads.findIndex(a => a.id === adId), 1);
      this.alertify.success('Обявата беше премахната от Наблюдавани');
    }, error => {
      this.alertify.error(error);
    });
  }
}
