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
  adParams: any = {};

  constructor(private categoryService: CategoryService, private adService: AdService,
              private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.ads = data.ads.result;
      this.pagination = data.ads.pagination;
    });

    this.getCategories();
    this.initAdParams();
  }

  getCategories() {
    this.categoryService.getAll().subscribe((categories: Category[]) => this.categories = categories,
    error => this.alertify.error(error));
  }

  initAdParams() {
    this.adParams.searchText = '';
    this.adParams.categoryId = 0;
    this.adParams.sortCriteria = 'newest';
  }

  pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    this.loadAds(false);
  }

  loadAds(returnToFirstPage: boolean) {
    if (returnToFirstPage) {
      this.pagination.currentPage = 1;
    }

    this.adService.getAds(this.pagination.currentPage, this.pagination.itemsPerPage, this.adParams)
      .subscribe((res: PaginatedResult<Ad[]>) => {
      this.ads = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

  filterByNameOrAddress() {
    if (this.adParams.searchText.replace(/\s/g, '').length) {
      this.adParams.searchText = this.adParams.searchText.trim().toLowerCase();
    }
    this.loadAds(true);
  }
}
