import { Component, OnInit } from '@angular/core';
import { AdService } from '../_services/ad.service';
import { Ad } from '../_models/ad';
import { AlertifyService } from '../_services/alertify.service';
import { PaginatedResult } from '../_models/pagination';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  ads: Ad[] = [];
  showSpinner = true;

  constructor(private adService: AdService,
              private alertify: AlertifyService,
              public authService: AuthService) { }

  ngOnInit() {
    this.loadAds();
  }

  loadAds() {
    const pageNumber = 1;
    const pageSize = 18;

    this.adService.getAds(pageNumber, pageSize).subscribe((ads: PaginatedResult<Ad[]>) => {
      this.shuffle(ads.result);
      this.ads = ads.result.slice(0, 6);
      this.showSpinner = false;
    }, error => {
      this.alertify.error(error);
      this.showSpinner = false;
    });
  }

  shuffle(a: Ad[]) {
    let i: number;
    let j: number;
    let temp: Ad;
    for (i = a.length - 1; i > 0; i--) {
        j = Math.floor(Math.random() * (i + 1));
        temp = a[i];
        a[i] = a[j];
        a[j] = temp;
    }
    return a;
  }
}
