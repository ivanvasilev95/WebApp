import { Component, OnInit } from '@angular/core';
import { Ad } from 'src/app/_models/ad';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AdService } from 'src/app/_services/ad.service';
import { Category } from 'src/app/_models/category';
import { AuthService } from 'src/app/_services/auth.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-ad-add',
  templateUrl: './ad-add.component.html',
  styleUrls: ['./ad-add.component.css']
})
export class NewAdComponent implements OnInit {
  categories: Category[];
  createAdForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private alertify: AlertifyService,
              private adService: AdService, private authService: AuthService,
              private route: ActivatedRoute, private location: Location) { }

  ngOnInit() {
    this.getCategories();
    this.createNewAdForm();
  }

  getCategories() {
    this.route.data.subscribe(data => {
      this.categories = data.categories;
    });
  }

  createNewAdForm() {
    this.createAdForm = this.fb.group({
      title: [''],
      categoryId: ['', Validators.required],
      description: [''],
      location: [''],
      price: [''],
      isUsed: ['']
    }, {validator: [this.titleValidator, this.descriptionValidator, this.locationValidator, this.priceValidator, this.isUsedValidator]});
  }

  titleValidator(g: FormGroup) {
    const title = g.get('title').value.trim();
    if (title.length < 3 || title.length > 50) {
      return {titleMismatch: true};
    }
    return null;
  }

  descriptionValidator(g: FormGroup) {
    const description = g.get('description').value.trim();
    if (description !== '' && (description.length < 10 || description.length > 1000)) {
      return {descriptionMismatch: true};
    }
    return null;
  }

  locationValidator(g: FormGroup) {
    const location = g.get('location').value.trim();
    if (location.length < 3 || location.length > 35) {
      return {locationMismatch: true};
    }
    return null;
  }

  priceValidator(g: FormGroup) {
    return g.get('price').value >= 0 ? null : {priceMismatch: true};
  }

  isUsedValidator(g: FormGroup) {
    return g.get('isUsed').value !== '' ? null : {isUsedMismatch: true};
  }

  get categoryId() {
    return this.createAdForm.get('categoryId');
  }

  get condition() {
    return this.createAdForm.get('isUsed');
  }

  selectCategory(e) {
    if (typeof(e.target.value) === 'number') {
      this.categoryId.setValue(e.target.value, {
        onlySelf: true
      });
    } else {
      this.categoryId.setValue(+e.target.value.substring(3), {
        onlySelf: true
      });
    }
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

  createAd() {
    if (this.createAdForm.valid) {
      const ad: Ad = Object.assign({}, this.createAdForm.value);
      ad.userId = +this.authService.decodedToken.nameid;
      ad.title = ad.title.trim();
      ad.location = ad.location.trim();
      ad.description = ad.description.trim() === '' ? null : ad.description.trim();

      this.adService.createAd(ad).subscribe(newAdId => {
        this.alertify.success('Обявата е създадена успешно');
        this.router.navigate(['/ads/' + newAdId + '/edit']);
      }, error => {
        this.alertify.error(error);
      });
    }
  }

  cancel() {
    this.location.back();
  }
}
