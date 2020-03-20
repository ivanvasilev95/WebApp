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
  pagination: Pagination;

  categories: Category[];
  // tslint:disable-next-line: no-inferrable-types
  categoryId: number = 0;
  // tslint:disable-next-line: no-inferrable-types
  sortCriteria: string = '';
  @ViewChild('searchBar', {static: false}) searchBar: ElementRef;

  constructor(private adService: AdService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.ads = data['ads'].result;
      this.shuffle(this.ads);
      // tslint:disable-next-line: no-string-literal
      this.pagination = data['ads'].pagination;
    });
    this.getCategories();
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
      console.log(error);
      // this.alertify.error(error);
    });
  }

  getCategories() {
    this.adService.getCategories().subscribe((categories: Category[]) => this.categories = categories,
    error => this.alertify.error(error));
  }

  search() {
    if (this.searchBar.nativeElement.value.replace(/\s/g, '').length) {
      console.log(this.searchBar.nativeElement.value);
    } else {
      // input contains only whitespace
    }

    if (this.categoryId !== 0) {
      console.log(this.categoryId);
    }

    if (this.sortCriteria !== '') {
      console.log(this.sortCriteria);
    }
}

  onCategorySelect(e) {
    this.categoryId = +e.target.value;

    if (this.categoryId === 0) {
      // all categories
    } else {
      console.log(this.categoryId);
    }

    if (this.searchBar.nativeElement.value.replace(/\s/g, '').length) {
      console.log(this.searchBar.nativeElement.value);
    }

    if (this.sortCriteria !== '') {
      console.log(this.sortCriteria);
    }
  }

  onSortSelect(e) {
    this.sortCriteria = e.target.value;
    console.log(this.sortCriteria);

    if (this.searchBar.nativeElement.value.replace(/\s/g, '').length) {
      console.log(this.searchBar.nativeElement.value);
    }

    if (this.categoryId !== 0) {
      console.log(this.categoryId);
    }
  }

  /*
  search() {
    if (this.searchBar.nativeElement.value !== '') {
      console.log(this.searchBar.nativeElement.value);

      if (this.categoryId !== 0) {
        console.log(this.categoryId);
      }

      if (this.sortCriteria !== '') {
        console.log(this.sortCriteria);
      }
    }
  }

  onCategorySelect(e) {
    this.categoryId = +e.target.value;

    if (this.categoryId === 0) {
      // all categories
    } else {
      console.log(this.categoryId);
    }

    if (this.searchBar.nativeElement.value !== '') {
      console.log(this.searchBar.nativeElement.value);
    }

    if (this.sortCriteria !== '') {
      console.log(this.sortCriteria);
    }
  }

  onSortSelect(e) {
    this.sortCriteria = e.target.value;
    console.log(this.sortCriteria);

    if (this.searchBar.nativeElement.value !== '') {
      console.log(this.searchBar.nativeElement.value);
    }

    if (this.categoryId !== 0) {
      console.log(this.categoryId);
    }
  }
  */
}
