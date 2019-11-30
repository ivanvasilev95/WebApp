import { Component, OnInit } from '@angular/core';
import { Ad } from '../_models/ad';
import { AdService } from '../_services/ad.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-user-ads',
  templateUrl: './user-ads.component.html',
  styleUrls: ['./user-ads.component.css']
})
export class UserAdsComponent implements OnInit {
  userAds: Ad[];

  constructor(private adService: AdService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadUserAds();
  }

  loadUserAds() {
    this.adService.getUserAds().subscribe((userAds: Ad[]) => {
      this.userAds = userAds;
    }, error => {
      console.log(error);
      // this.alertify.error(error);
    });
  }
}
