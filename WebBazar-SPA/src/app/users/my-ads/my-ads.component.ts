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
      this.alertify.error(error); // 'Неуспешно зареждане на обявите'
    });
  }

  deleteAd(adId: number) {
    if (confirm('Сигурни ли сте, че искате да изтриете тази обява?')) {
      this.adService.deleteAd(adId).subscribe(() => {
        this.userAds.splice(this.userAds.findIndex(a => a.id === adId), 1);
        this.alertify.success('Обявата е премахната успешно');
      }, error => {
        this.alertify.error(error);
      });
    }
  }
}
