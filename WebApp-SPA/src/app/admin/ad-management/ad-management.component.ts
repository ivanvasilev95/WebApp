import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-ad-management',
  templateUrl: './ad-management.component.html',
  styleUrls: ['./ad-management.component.css']
})
export class AdManagementComponent implements OnInit {
  ads: any;

  constructor(private adminService: AdminService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.getAdsForApproval();
  }

  getAdsForApproval() {
    this.adminService.getAdsForApproval().subscribe(ads => {
      this.ads = ads;
    },
    error => {
      this.alertify.error(error);
    });
  }

  approveAd(adId) {
    this.adminService.approveAd(adId).subscribe(() => {
      this.ads.splice(this.ads.findIndex(a => a.id === adId), 1);
    },
    error => {
      this.alertify.error(error);
    });
  }

  rejectAd(adId) {
    this.adminService.rejectAd(adId).subscribe(() => {
      this.ads.splice(this.ads.findIndex(a => a.id === adId), 1);
    },
    error => {
      this.alertify.error(error);
    });
  }
}
