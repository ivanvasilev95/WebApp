import { Component, OnInit } from '@angular/core';
import { AdService } from '../../_services/ad.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Ad } from '../../_models/ad';
import { ActivatedRoute } from '@angular/router';
import { Category } from 'src/app/_models/category';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { CategoryService } from 'src/app/_services/category.service';

@Component({
  selector: 'app-ad-list',
  templateUrl: './ad-list.component.html',
  styleUrls: ['./ad-list.component.css']
})
export class AdListComponent implements OnInit {
  ads: Ad[];
  categories: Category[];
  pagination: Pagination;
  userParams: any = {};

  constructor(private categoryService: CategoryService, private adService: AdService,
              private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.ads = data.ads.result;
      this.pagination = data.ads.pagination;
    });

    this.getCategories();
    this.initUserParams();
  }

  getCategories() {
    this.categoryService.getCategories().subscribe((categories: Category[]) => this.categories = categories,
    error => this.alertify.error(error));
  }

  initUserParams() {
    this.userParams.searchText = '';
    this.userParams.categoryId = 0;
    this.userParams.sortCriteria = 'newest';
  }

  pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    this.loadAds(false);
  }

  loadAds(flag: boolean) {
    if (flag === true) { // return to first page
      this.pagination.currentPage = 1;
    }
    this.adService.getAds(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe((res: PaginatedResult<Ad[]>) => {
      this.ads = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

  filterByNameOrAddress() {
    if (this.userParams.searchText.replace(/\s/g, '').length) {
      this.userParams.searchText = this.userParams.searchText.trim().toLowerCase();
    }
    this.loadAds(true);
  }
}
