import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Category } from '../_models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCategories() {
    return this.http.get<Category[]>(this.baseUrl + 'categories');
  }

  addCategory(category: any) {
    return this.http.post(this.baseUrl + 'categories', category);
  }

  removeCategory(id: number) {
    return this.http.delete(this.baseUrl + 'categories/' + id);
  }
}
