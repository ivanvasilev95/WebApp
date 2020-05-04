import { Component, OnInit } from '@angular/core';
import { AdService } from '../_services/ad.service';
import { Ad } from '../_models/ad';
import { AlertifyService } from '../_services/alertify.service';
import { PaginatedResult } from '../_models/pagination';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  ads: Ad[];
  pageNumber = 1;
  pageSize = 10;
  showSpinner = true;

  constructor(private adService: AdService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadAds();
  }

  loadAds() {
    this.adService.getAds(this.pageNumber, this.pageSize).subscribe((ads: PaginatedResult<Ad[]>) => {
      this.ads = ads.result;
      this.shuffle(this.ads);
      this.showSpinner = false;
    }, error => {
      this.alertify.error(error); // 'Грешка при зареждане на обявите'
      this.showSpinner = false;
    });
  }

  shuffle(a) {
    // tslint:disable-next-line: one-variable-per-declaration
    let j, x, i;
    for (i = a.length - 1; i > 0; i--) {
        j = Math.floor(Math.random() * (i + 1));
        x = a[i];
        a[i] = a[j];
        a[j] = x;
    }
    return a;
  }
}
