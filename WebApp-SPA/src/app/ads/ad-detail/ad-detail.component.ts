import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { Ad } from 'src/app/_models/ad';
import { AdService } from 'src/app/_services/ad.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

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
      this.ad = data.ad;
    });

    this.galleryOptions = [
      {
        width: '465px',
        height: '465px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];

    this.galleryImages = this.getImages();

    this.setSenderId();
    this.getAdLikesCount();
  }

  getImages() {
    const imageUrls = [];
    for (const photo of this.ad.photos) {
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url
      });
    }
    return imageUrls;
  }

  private setSenderId() {
    if (this.route.snapshot.paramMap.get('senderId') == null || this.route.snapshot.paramMap.get('senderId') === undefined) {
      this.senderId = null;
    } else {
      this.senderId = +this.route.snapshot.paramMap.get('senderId');
    }
  }

  getAdLikesCount() {
    this.adService.getAdLikesCount(this.ad.id).subscribe((count: number) => {
      this.adLikesCount = count;
    });
  }

  ngAfterViewInit() {
    this.setActiveTab();
  }

  private setActiveTab() {
    this.route.queryParams.subscribe(params => {
      const selectedTab = params.tab;
      this.adTabs.tabs[selectedTab > 0 && selectedTab !== undefined ? selectedTab : 0].active = true;
    });
  }

  addToLikedAds(adId: number) {
    this.adService.addAdToLiked(this.authService.decodedToken.nameid, adId).subscribe(data => {
      this.alertify.success('Вие добавихте ' + this.ad.title + ' в Наблюдавани');
    }, error => {
      this.alertify.error(error);
    });
  }

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
