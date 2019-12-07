import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdService } from 'src/app/_services/ad.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Ad } from 'src/app/_models/ad';

@Component({
  selector: 'app-ad-edit',
  templateUrl: './ad-edit.component.html',
  styleUrls: ['./ad-edit.component.css']
})
export class AdEditComponent implements OnInit {
  ad: Ad;

  constructor(private adService: AdService, private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.getAd();
  }

  getAd() {
    // tslint:disable-next-line: no-string-literal
    this.adService.getAd(+this.route.snapshot.params['id']).subscribe((ad: Ad) => this.ad = ad,
      error => this.alertify.error(error));
  }

  updateAd() {
    // this.adService.UpdateAd(adId).subscribe(next => {
      // this.alertify.success('Ad updated successfully');
      // reset form
    // }, error => {
      // this.alertify.error(error);
    // });
  }

  logAd() { console.log(this.ad); }

}
