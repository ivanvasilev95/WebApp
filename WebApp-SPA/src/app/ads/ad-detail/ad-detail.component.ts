import { Component, OnInit } from '@angular/core';
import { Ad } from 'src/app/_models/ad';
import { AdService } from 'src/app/_services/ad.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';

@Component({
  selector: 'app-ad-detail',
  templateUrl: './ad-detail.component.html',
  styleUrls: ['./ad-detail.component.css']
})
export class AdDetailComponent implements OnInit {
  ad: Ad;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private adService: AdService, private authService: AuthService,
              private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.ad = data['ad'];
    });

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];

    this.galleryImages = this.getImages();
  }

  getImages() {
    const imageUrls = [];
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < this.ad.photos.length; i++) {
      imageUrls.push({
        small: this.ad.photos[i].url,
        medium: this.ad.photos[i].url,
        big: this.ad.photos[i].url
      });
    }
    return imageUrls;
  }

  /*
  loadAd() {
    // tslint:disable-next-line: no-string-literal
    this.adService.getAd(+this.route.snapshot.params['id']).subscribe((ad: Ad) => {
      this.ad = ad;
    }, error => {
      console.log(error);
      // this.alertify.error(error);
    });
  }
  */
  isAuthorized(): boolean {
    return this.authService.loggedIn() && this.isNotUsersAd();
  }

  isNotUsersAd() {
    return +this.authService.decodedToken.nameid !== this.ad.userId;
  }

}
