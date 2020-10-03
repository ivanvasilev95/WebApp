import { Component, OnInit } from '@angular/core';
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

  constructor(private categoryService: CategoryService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.categoryService.getCategories().subscribe((categories: Category[]) => {
      this.categories = categories;
    },
    error => {
      this.alertify.error(error);
    });
  }

  addCategory() {
    const name = this.newCategoryName.trim();
    this.categoryService.addCategory({name}).subscribe((category: Category) => {
      this.alertify.success('Категорията е добавена успешно');
      this.categories.push(category);
      this.newCategoryName = '';
    },
    error => {
      this.alertify.error(error);
    });
  }

  removeCategory(categoryId: number) {
    this.alertify.confirm('Сигурни ли сте, че искате да изтриете тази категория?', () => {
      this.categoryService.removeCategory(categoryId).subscribe(res => {
        this.categories.splice(this.categories.findIndex(c => c.id === categoryId), 1);
        this.alertify.success('Категорията е премахната успешно');
      },
      error => {
        this.alertify.error(error);
      });
    });
  }
}
