import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AdService } from '../../_services/ad.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Ad } from '../../_models/ad';
import { ActivatedRoute } from '@angular/router';
import { Category } from 'src/app/_models/category';

@Component({
  selector: 'app-ad-list',
  templateUrl: './ad-list.component.html',
  styleUrls: ['./ad-list.component.css']
})
export class AdListComponent implements OnInit {
  ads: Ad[];
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
      this.ads = data['ads'];
      this.shuffle(this.ads);
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

  /*
  loadAds() {
    this.adService.getAds().subscribe((ads: Ad[]) => {
      this.ads = ads;
    }, error => {
      console.log(error);
      // this.alertify.error(error);
    });
  }
  */
}
