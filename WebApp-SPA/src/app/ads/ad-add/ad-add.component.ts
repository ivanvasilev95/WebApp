import { Component, OnInit } from '@angular/core';
import { Ad } from 'src/app/_models/ad';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AdService } from 'src/app/_services/ad.service';
import { Category } from 'src/app/_models/category';
import { AuthService } from 'src/app/_services/auth.service';
import { Location } from '@angular/common';
import { CategoryService } from 'src/app/_services/category.service';

@Component({
  selector: 'app-ad-add',
  templateUrl: './ad-add.component.html',
  styleUrls: ['./ad-add.component.css']
})
export class NewAddComponent implements OnInit {
  categories: Category[];
  createAdForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private alertify: AlertifyService,
              private adService: AdService, private authService: AuthService,
              private categoryService: CategoryService, private location: Location) { }

  ngOnInit() {
    this.getCategories();
    this.createNewAdForm();
  }

  getCategories() {
    this.categoryService.getCategories().subscribe(
      (categories: Category[]) => this.categories = categories,
      error => this.alertify.error(error));
  }

  get categoryId() {
    return this.createAdForm.get('categoryId');
  }

  selectCategory(e) {
    if (typeof(e.target.value) === 'number') {
      this.categoryId.setValue(e.target.value, {
        onlySelf: true
      });
    } else {
      this.categoryId.setValue(+e.target.value.substring(0, 1), {
        onlySelf: true
      });
    }
  }

  get condition() {
    return this.createAdForm.get('isUsed');
  }

  selectCondition(e) {
    if (e.target.value === 'null') {
      this.condition.setValue(null, {
        onlySelf: true
      });
    } else {
      this.condition.setValue(e.target.value, {
        onlySelf: true
      });
    }
  }

  createNewAdForm() {
    this.createAdForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      categoryId: ['', Validators.required],
      description: ['', Validators.maxLength(1000)],
      location: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(35)]],
      price: [''],
      isUsed: [null]
    }, {validator: this.priceValidator});
  }

  priceValidator(g: FormGroup) {
    return g.get('price').value >= 0 ? null : {mismatch: true};
  }

  createAd() {
    if (this.createAdForm.valid) {
      const ad: Ad = Object.assign({}, this.createAdForm.value);
      ad.userId = +this.authService.decodedToken.nameid;
      ad.isApproved = false; // maybe not necessary
      ad.categoryName = this.categories.find(c => c.id === ad.categoryId).name; // maybe not necessary

      this.adService.createAd(ad).subscribe((newAd: Ad) => {
        this.alertify.success('Обявата е създадена успешно.');
        this.router.navigate(['/user/ad/' + newAd.id + '/edit']);
      }, error => {
        this.alertify.error(error); // 'Грешка при създаването на обявата.'
      });
    }
  }

  cancel() {
    this.location.back();
  }
}
