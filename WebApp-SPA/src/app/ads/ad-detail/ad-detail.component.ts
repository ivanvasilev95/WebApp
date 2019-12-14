import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { Ad } from 'src/app/_models/ad';
import { AdService } from 'src/app/_services/ad.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap';

@Component({
  selector: 'app-ad-detail',
  templateUrl: './ad-detail.component.html',
  styleUrls: ['./ad-detail.component.css']
})
export class AdDetailComponent implements OnInit, AfterViewInit {
  ad: Ad;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  @ViewChild('adTabs', {static: false}) adTabs: TabsetComponent;
  senderId: number; // should have been recipientId
  adLikesCount: number;

  constructor(private adService: AdService, public authService: AuthService,
              private alertify: AlertifyService, private route: ActivatedRoute, private router: Router) { }

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

    if (this.route.snapshot.paramMap.get('senderId') == null || this.route.snapshot.paramMap.get('senderId') === undefined) {
      this.senderId = null;
    } else {
    this.senderId = +this.route.snapshot.paramMap.get('senderId');
    }

    this.getAdLikesCount();
  }

  ngAfterViewInit() {
    this.route.queryParams.subscribe(params => {
      // tslint:disable-next-line: no-string-literal
      const selectedTab = params['tab'];
      this.adTabs.tabs[selectedTab > 0 && selectedTab !== undefined ? selectedTab : 0].active = true;
    });
  }

  getAdLikesCount() {
    this.adService.getAdLikesCount(this.ad.id).subscribe((count: number) => {
      this.adLikesCount = count;
    });
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

  addToFavorites(adId: number) {
    this.adService.addToFavorites(this.authService.decodedToken.nameid, adId).subscribe(data => {
      this.alertify.success('You have added ' + this.ad.title + ' to favorites');
      // this.getAdLikesCount();
    }, error => {
      // this.alertify.error(error);
      this.alertify.error('This ad has already beed added to Favorites');
    });
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
    return this.isLoggedIn() && this.isNotUsersAd();
  }

  isNotUsersAd(): boolean {
    return +this.authService.decodedToken.nameid !== this.ad.userId;
  }

  isLoggedIn(): boolean {
    return this.authService.loggedIn();
  }

  selectTab(tabId: number) {
    this.adTabs.tabs[tabId].active = true;
  }
}
