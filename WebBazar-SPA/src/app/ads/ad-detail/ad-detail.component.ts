import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { Ad } from 'src/app/_models/ad';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { LikeService } from 'src/app/_services/like.service';

@Component({
  selector: 'app-ad-detail',
  templateUrl: './ad-detail.component.html',
  styleUrls: ['./ad-detail.component.css']
})
export class AdDetailComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('adTabs', {static: false}) adTabs: TabsetComponent;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  ad: Ad;
  recipientId: number;

  constructor(private likeService: LikeService, public authService: AuthService,
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
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.setActiveTab();
    });
  }

  ngOnDestroy() {
    if (localStorage.getItem('recipientId') !== null) {
      localStorage.removeItem('recipientId');
    }
    if (localStorage.getItem('tabNumber') !== null) {
      localStorage.removeItem('tabNumber');
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
    if (localStorage.getItem('recipientId') !== null) {
      this.recipientId = +localStorage.getItem('recipientId');
    } else {
      this.recipientId = this.ad.userId;
    }
  }

  private setActiveTab() {
    if (localStorage.getItem('tabNumber') !== null) {
      this.adTabs.tabs[+localStorage.getItem('tabNumber')].active = true;
    } else {
      this.adTabs.tabs[0].active = true;
    }
  }

  saveTabNumberInLocalStorage(tabNumber: number) {
    localStorage.setItem('tabNumber', tabNumber.toString());
  }

  likeAd(adId: number) {
    this.likeService.likeAd(adId).subscribe(() => {
      this.alertify.success('Вие добавихте ' + this.ad.title + ' в Наблюдавани');
    }, error => {
      this.alertify.error(error);
    });
  }

  isNotLoggedInUserAd() {
    return this.ad.userId !== this.getLoggedInUserId();
  }

  getLoggedInUserId() {
    return +this.authService.decodedToken.nameid;
  }

  userIsNotAdminOnly() {
    const userRoles = this.userIsLoggedIn() ? this.authService.decodedToken.role as Array<string> : null;
    if (userRoles && !(userRoles instanceof Array) && userRoles === 'Admin') {
      return false;
    }
    return true;
  }

  userIsLoggedIn() {
    return this.authService.loggedIn();
  }
}
