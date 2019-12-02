import { Component, OnInit } from '@angular/core';
import { AdService } from '../../_services/ad.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Ad } from '../../_models/ad';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ad-list',
  templateUrl: './ad-list.component.html',
  styleUrls: ['./ad-list.component.css']
})
export class AdListComponent implements OnInit {
  ads: Ad[];

  constructor(private adService: AdService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.ads = data['ads'];
    });
  }

  /*
  loadAds() {
    this.adService.getAds().subscribe((ads: Ad[]) => {
      this.ads = ads;
    }, error => {
      console.log(error);
      // this.alertify.error(error);
    });
  }
  */
}
