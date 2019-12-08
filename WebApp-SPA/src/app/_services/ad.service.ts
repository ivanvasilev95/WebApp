import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Ad } from '../_models/ad';
import { Observable } from 'rxjs';
import { Category } from '../_models/category';

/*
const httpOptions = {
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + localStorage.getItem('token') //
  })
};
*/

@Injectable({
  providedIn: 'root'
})
export class AdService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAds(): Observable<Ad[]> {
    return this.http.get<Ad[]>(this.baseUrl + 'ads');
  }

  getAd(id: number): Observable<Ad> {
    return this.http.get<Ad>(this.baseUrl + 'ads/' + id);
  }

  getUserAds(): Observable<Ad[]> {
    return this.http.get<Ad[]>(this.baseUrl + 'ads/user'/*, httpOptions*/);
  }

  deleteAd(id) {
    return this.http.delete(this.baseUrl + 'ads/' + id);
  }

  getCategories() {
    return this.http.get<Category[]>(this.baseUrl + 'ads/categories');
  }

  updateAd(id: number, ad: Ad) {
    return this.http.put(this.baseUrl + 'ads/' + id, ad);
  }

  createAd(ad: Ad) {
    return this.http.post(this.baseUrl + 'ads', ad);
  }
}
