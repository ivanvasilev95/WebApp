import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { Ad } from 'src/app/_models/ad';
import { AdService } from 'src/app/_services/ad.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-ad-detail',
  templateUrl: './ad-detail.component.html',
  styleUrls: ['./ad-detail.component.css']
})
export class AdDetailComponent implements OnInit, AfterViewInit {
  @ViewChild('adTabs', {static: false}) adTabs: TabsetComponent;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  ad: Ad;
  recipientId: number;
  adLikesCount: number = null;

  constructor(private adService: AdService, public authService: AuthService,
              private alertify: AlertifyService, private route: ActivatedRoute) { }

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

    this.setRecipientId();
    if (this.ad.isApproved && this.userIsLoggedIn() && !this.isNotLoggedInUserAd()) {
      this.getAdLikesCount();
    }
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

  private setRecipientId() {
    this.route.queryParams.subscribe(params => {
      const recipientId = params.recipient;
      if (recipientId !== undefined) {
        this.recipientId = +recipientId;
      } else {
        this.recipientId = null;
      }
    });
  }

  getAdLikesCount() {
    this.adService.getAdLikesCount(this.ad.id).subscribe((count: number) => {
      this.adLikesCount = count;
    });
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.setActiveTab();
    });
  }

  private setActiveTab() {
    this.route.queryParams.subscribe(params => {
      const selectedTab = params.tab;
      this.adTabs.tabs[selectedTab > 0 && selectedTab !== undefined ? selectedTab : 0].active = true;
    });
  }

  addToLikedAds(adId: number) {
    this.adService.addAdToLiked(adId).subscribe(data => {
      this.alertify.success('Вие добавихте ' + this.ad.title + ' в Наблюдавани');
    }, error => {
      this.alertify.error(error);
    });
  }

  userHasRights(): boolean {
    return this.userIsLoggedIn() && this.isNotLoggedInUserAd();
  }

  isNotLoggedInUserAd(): boolean {
    return +this.authService.decodedToken.nameid !== this.ad.userId;
  }

  userIsLoggedIn(): boolean {
    return this.authService.loggedIn();
  }

  selectTab(tabId: number) {
    this.adTabs.tabs[tabId].active = true;
  }
}
