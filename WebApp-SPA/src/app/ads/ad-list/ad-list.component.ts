import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AdService } from '../../_services/ad.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Ad } from '../../_models/ad';
import { ActivatedRoute } from '@angular/router';
import { Category } from 'src/app/_models/category';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-ad-list',
  templateUrl: './ad-list.component.html',
  styleUrls: ['./ad-list.component.css']
})
export class AdListComponent implements OnInit {
  ads: Ad[];
  adsToFilter: Ad[];
  categories: Category[];
  pagination: Pagination;

  // tslint:disable-next-line: no-inferrable-types
  searchText: string = '';
  // tslint:disable-next-line: no-inferrable-types
  categoryId: string = '0';
  // tslint:disable-next-line: no-inferrable-types
  sortCriteria: string = 'newest';

  constructor(private adService: AdService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.ads = data['ads'].result;
      // tslint:disable-next-line: no-string-literal
      this.pagination = data['ads'].pagination;
      this.shuffle(this.ads);
      this.adsToFilter = this.ads.map(x => Object.assign({}, x));
    });
    this.getCategories();
    this.sortAds();
  }

  filterAds() {
    this.filterByCategory(this.adsToFilter);
    this.filterByNameOrAddress(this.ads);
    this.sortAds();
  }

  filterByCategory(ads) {
    if (+this.categoryId !== 0) {
      this.ads = ads.filter(ad => ad.categoryId === +this.categoryId);
    } else { // all categories
      this.ads = ads.map(x => Object.assign({}, x));
    }
  }

  filterByNameOrAddress(ads) {
    if (this.searchText.replace(/\s/g, '').length) {
      this.searchText = this.searchText.trim().toLowerCase();
      this.ads = ads.filter(ad => ad.title.toLowerCase().includes(this.searchText) || ad.location.toLowerCase().includes(this.searchText));
    } else { // input contains only whitespace
      this.ads = ads.map(x => Object.assign({}, x));
    }
  }

  sortAds() {
    switch (this.sortCriteria) {
      case 'newest': {
        this.ads.sort((a, b) => new Date(b.dateAdded).getTime() - new Date(a.dateAdded).getTime());
        break;
      }
      case 'cheapest': {
        this.ads = this.ads.filter(ad => ad.price !== null);
        this.ads.sort((a, b) => a.price - b.price);
        break;
      }
      case 'expensive': {
        this.ads = this.ads.filter(ad => ad.price !== null);
        this.ads.sort((a, b) => b.price - a.price);
        break;
      }
      default: { // po dogovarqne
        this.ads = this.ads.filter(ad => ad.price === null);
        break;
      }
    }
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

  pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    this.loadAds();
  }

  loadAds() {
    this.adService.getAds(this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe((res: PaginatedResult<Ad[]>) => {
      this.ads = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

  getCategories() {
    this.adService.getCategories().subscribe((categories: Category[]) => this.categories = categories,
    error => this.alertify.error(error));
  }
}
