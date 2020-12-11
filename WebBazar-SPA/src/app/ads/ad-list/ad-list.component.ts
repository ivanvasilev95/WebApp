import { Component, OnInit } from '@angular/core';
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
  categories: Category[];
  pagination: Pagination;
  adParams: any = { searchText: '', categoryId: 0, sortCriteria: 'newest'};

  constructor(private adService: AdService,
              private alertify: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.ads = data.ads.result;
      this.pagination = data.ads.pagination;
      this.categories = data.categories;
    });
  }

  filterByNameOrAddress() {
    this.adParams.searchText = this.adParams.searchText.trim().toLowerCase();
    this.loadAds(true);
  }

  pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    this.loadAds(false);
  }

  loadAds(returnToFirstPage: boolean) {
    if (returnToFirstPage && this.pagination.currentPage > 1) {
      this.pageChanged({page: 1}); // this.pagination.currentPage = 1;
    } else {
      this.adService.getAds(this.pagination.currentPage, this.pagination.itemsPerPage, this.adParams)
        .subscribe((res: PaginatedResult<Ad[]>) => {
        this.ads = res.result;
        this.pagination = res.pagination;
      }, error => {
        this.alertify.error(error);
      });
    }
  }

  getPaginator() {
    const pageItems = this.pagination.currentPage * this.pagination.itemsPerPage;

    return 'Показване на съобщения ' +
           (1 + (this.pagination.itemsPerPage * (this.pagination.currentPage - 1))) +
           ' - ' +
           (pageItems > this.pagination.totalItems ? this.pagination.totalItems : pageItems) +
           ' от ' +
           this.pagination.totalItems;
  }
}
