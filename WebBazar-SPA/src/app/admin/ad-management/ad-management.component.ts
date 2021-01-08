import { Component, OnInit } from '@angular/core';
import { Ad } from 'src/app/_models/ad';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-ad-management',
  templateUrl: './ad-management.component.html',
  styleUrls: ['./ad-management.component.css']
})
export class AdManagementComponent implements OnInit {
  ads: Ad[];

  constructor(private adminService: AdminService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.getAdsForApproval();
  }

  getAdsForApproval() {
    this.adminService.getAdsForApproval().subscribe((ads: Ad[]) => {
      this.ads = ads;
    },
    error => {
      this.alertify.error(error);
    });
  }

  approveAd(adId) {
    this.adminService.approveAd(adId).subscribe(() => {
      this.alertify.success(this.ads.find(a => a.id === adId).title + ' беше одобрена');
      this.ads.splice(this.ads.findIndex(a => a.id === adId), 1);
    },
    error => {
      this.alertify.error(error);
    });
  }

  rejectAd(adId) {
    this.adminService.rejectAd(adId).subscribe(() => {
      this.alertify.error(this.ads.find(a => a.id === adId).title + ' беше отхвърлена');
      this.ads.splice(this.ads.findIndex(a => a.id === adId), 1);
    },
    error => {
      this.alertify.error(error);
    });
  }
}
