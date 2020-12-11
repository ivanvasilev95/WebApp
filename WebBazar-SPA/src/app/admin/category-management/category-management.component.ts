import { Component, OnInit, ViewChild } from '@angular/core';
import { Category } from 'src/app/_models/category';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { CategoryService } from 'src/app/_services/category.service';

@Component({
  selector: 'app-category-management',
  templateUrl: './category-management.component.html',
  styleUrls: ['./category-management.component.css']
})
export class CategoryManagementComponent implements OnInit {
  categories: Category[];
  newCategoryName = '';
  allowEdit = false;

  constructor(private categoryService: CategoryService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.getCategories();
  }

  getCategories() {
    this.categoryService.getAll().subscribe((categories: Category[]) => {
      this.categories = categories;
    },
    error => {
      this.alertify.error(error);
    });
  }

  addCategory() {
    const name = this.newCategoryName.trim();

    if (this.categories.some(c => c.name.toLowerCase() === name.toLowerCase())) {
      this.alertify.error('Категорията вече съществува.');
      return;
    }

    this.categoryService.add({name}).subscribe((category: Category) => {
      this.categories.push(category);
      this.newCategoryName = '';
      this.alertify.success('Категорията е добавена успешно.');
    },
    error => {
      this.alertify.error(error);
    });
  }

  removeCategory(categoryId: number) {
    this.alertify.confirm('Сигурни ли сте, че искате да изтриете тази категория?', () => {
      this.categoryService.remove(categoryId).subscribe(() => {
        this.categories.splice(this.categories.findIndex(c => c.id === categoryId), 1);
        this.alertify.success('Категорията е премахната успешно.');
      },
      error => {
        this.alertify.error(error);
      });
    });
  }

  updateCategory(event, category) {
    const changedCategoryName = event.target.innerText.trim();

    if (!this.categoryNameIsValid(changedCategoryName)) {
      event.target.innerText = category.name;
      return;
    }

    if (changedCategoryName !== category.name) {
      if (this.categories.some(c => c.name.toLowerCase() === changedCategoryName.toLowerCase())) {
        event.target.innerText = category.name;
        this.alertify.error('Категорията вече съществува.');
        return;
      } else {
        this.categoryService.update(category.id, {name: changedCategoryName}).subscribe(() => {
            category.name = changedCategoryName;
            this.alertify.success('Категорията е преименувана успешно.');
          },
          error => {
            event.target.innerText = category.name;
            this.alertify.error(error);
          });
      }
    } else {
      event.target.innerText = changedCategoryName;
    }

    this.allowEdit = false;
  }

  categoryNameIsValid(categoryName) {
    if (categoryName.length === 0) {
      this.alertify.error('Полето не може да бъде празно.');
      return false;
    }

    if (categoryName.length < 4 || categoryName.length > 20) {
      this.alertify.error('Името на категорията не трябва да бъде по-малко от 4 или повече от 20 символа.');
      return false;
    }

    return true;
  }

  enableEdit() {
    this.allowEdit = true;
  }
}
