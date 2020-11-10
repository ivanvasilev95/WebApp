import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Category } from '../_models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  baseUrl = environment.apiUrl + 'categories/';

  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get<Category[]>(this.baseUrl);
  }

  add(category: any) {
    return this.http.post(this.baseUrl, category);
  }

  update(id: number, category: any) {
    return this.http.put(this.baseUrl + id, category);
  }

  remove(id: number) {
    return this.http.delete(this.baseUrl + id);
  }
}
