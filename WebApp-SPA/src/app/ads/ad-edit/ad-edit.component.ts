import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdService } from 'src/app/_services/ad.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Ad } from 'src/app/_models/ad';
import { Category } from 'src/app/_models/category';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-ad-edit',
  templateUrl: './ad-edit.component.html',
  styleUrls: ['./ad-edit.component.css']
})
export class AdEditComponent implements OnInit {
  ad: Ad;
  categories: Category[];
  @ViewChild('editForm', {static: false}) editForm: NgForm;

  constructor(private adService: AdService, private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.getAd();
    this.getCategories();
  }

  getAd() {
    // tslint:disable-next-line: no-string-literal
    this.adService.getAd(+this.route.snapshot.params['id']).subscribe((ad: Ad) => this.ad = ad,
      error => this.alertify.error(error));
  }

  updateAd() {
    this.adService.updateAd(this.ad.id, this.ad).subscribe(next => {
      this.alertify.success('Обявата е редактирана успешно');
      this.editForm.reset(this.ad);
    }, error => {
      this.alertify.error(error);
    });
  }

  getCategories() {
    this.adService.getCategories().subscribe((categories: Category[]) => this.categories = categories,
    error => this.alertify.error(error));
  }

  onCategorySelect() {
    this.ad.categoryName = this.categories.find(c => c.id === this.ad.categoryId).name;
  }

  updateMainPhoto(photoUrl) {
    this.ad.photoUrl = photoUrl;
  }
}
