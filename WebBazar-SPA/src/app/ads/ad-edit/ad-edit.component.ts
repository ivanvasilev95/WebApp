import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdService } from 'src/app/_services/ad.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Ad } from 'src/app/_models/ad';
import { Category } from 'src/app/_models/category';
import { NgForm } from '@angular/forms';
import { CategoryService } from 'src/app/_services/category.service';

@Component({
  selector: 'app-ad-edit',
  templateUrl: './ad-edit.component.html',
  styleUrls: ['./ad-edit.component.css']
})
export class AdEditComponent implements OnInit {
  ad: Ad;
  categories: Category[];
  @ViewChild('editForm', {static: false}) editForm: NgForm;
  @ViewChild('descriptionForm', {static: false}) descriptionForm: NgForm;

  constructor(private adService: AdService, private categoryService: CategoryService,
              private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.getAd();
    this.getCategories();
  }

  getAd() {
    this.route.data.subscribe(data => {
      this.ad = data.ad;
    });
  }

  getCategories() {
    this.categoryService.getAll().subscribe(
      (categories: Category[]) => this.categories = categories,
      error => this.alertify.error(error)
    );
  }

  updateAd() {
    let fieldsAreValid = true;

    if (this.validateField(this.ad.title, 'Заглавие', 3, 50)) {
      this.ad.title = this.ad.title.trim();
    } else {
      fieldsAreValid = false;
    }

    if (this.validateField(this.ad.location, 'Адрес', 3, 35)) {
      this.ad.location = this.ad.location.trim();
    } else {
      fieldsAreValid = false;
    }

    if (!fieldsAreValid) {
      return;
    }

    this.adService.updateAd(this.ad.id, this.ad).subscribe(() => {
      this.alertify.success('Обявата е редактирана успешно');
      this.editForm.reset(this.ad);
      this.descriptionForm.reset(this.ad);
    }, error => {
      this.alertify.error(error);
    });
  }

  validateField(field, title, min, max) {
    if (field === null || field.trim() === '') {
      this.alertify.error('Полето \'' + title + '\' е задължително');
      return false;
    }

    if (field.trim().length < min || field.trim().length > max) {
      this.alertify.error('Полето \'' + title + '\' не трябва да бъде по-късо от ' + min + ' или по-дълго от ' + max + ' символа');
      return false;
    }

    return true;
  }

  updateDescription() {
    if (this.ad.description !== null && this.ad.description.trim() !== '') {
      if (this.ad.description.trim().length < 10 || this.ad.description.trim().length > 1000) {
        this.alertify.error('Полето \'Описание\' не трябва да бъде по-късо от 10 или по-дълго от 1000 символа');
        return;
      } else {
        this.ad.description = this.ad.description.trim();
      }
    } else {
      this.ad.description = null;
    }

    this.updateAd();
  }

  onCategorySelect() {
    this.ad.categoryName = this.categories.find(c => c.id === this.ad.categoryId).name;
  }

  updateMainPhoto(photoUrl: string) {
    this.ad.photoUrl = photoUrl;
  }

  resetForm(tab: string) {
    this.adService.getAd(this.ad.id).subscribe(ad => { this.ad = ad; });
    if (tab === 'description') {
      this.descriptionForm.reset(this.ad.description);
    }
    if (tab === 'details') {
      this.editForm.reset(this.ad);
    }
    if (tab === 'both') {
      if (this.editForm.dirty === true) {
        this.editForm.reset(this.ad);
      }
      if (this.descriptionForm.dirty === true) {
        this.descriptionForm.reset(this.ad.description);
      }
    }
  }
}
