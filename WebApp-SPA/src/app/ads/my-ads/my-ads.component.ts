import { Component, OnInit } from '@angular/core';
import { Ad } from '../../_models/ad';
import { AdService } from '../../_services/ad.service';
import { AlertifyService } from '../../_services/alertify.service';

@Component({
  selector: 'app-my-ads',
  templateUrl: './my-ads.component.html',
  styleUrls: ['./my-ads.component.css']
})
export class MyAdsComponent implements OnInit {
  userAds: Ad[];

  constructor(private adService: AdService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadUserAds();
  }

  loadUserAds() {
    this.adService.getUserAds().subscribe((userAds: Ad[]) => {
      this.userAds = userAds;
    }, error => {
      // console.log(error);
      this.alertify.error('Неуспешно зареждане на обявите');
    });
  }

  onDelete(adId) {
    if (confirm('Сигурни ли сте, че искате да изтриете тази обява?')) {
      this.adService.deleteAd(adId).subscribe(res => {
        this.loadUserAds();
        this.alertify.success('Обявата е премахната успешно');
      }, error => {
        console.log(error);
        // this.alertify.error(error);
      });
    }
  }
}
