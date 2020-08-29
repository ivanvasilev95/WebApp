import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Ad } from '../_models/ad';
import { Observable } from 'rxjs';
import { Category } from '../_models/category';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

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

  getAds(page?, itemsPerPage?, userParams?): Observable<PaginatedResult<Ad[]>> {
    const paginatedResult: PaginatedResult<Ad[]> = new PaginatedResult<Ad[]>();

    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userParams != null) {
      params = params.append('searchText', userParams.searchText);
      params = params.append('categoryId', userParams.categoryId);
      params = params.append('sortCriteria', userParams.sortCriteria);
    }

    return this.http.get<Ad[]>(this.baseUrl + 'ads', { observe: 'response', params})
    .pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
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
    return this.http.get<Category[]>(this.baseUrl + 'categories');
  }

  updateAd(id: number, ad: Ad) {
    return this.http.put(this.baseUrl + 'ads/' + id, ad);
  }

  createAd(ad: Ad) {
    return this.http.post(this.baseUrl + 'ads', ad);
  }

  addToFavorites(userId: number, adId: number) {
    return this.http.post(this.baseUrl + 'likes/user/' + userId + '/ad/' + adId, {});
  }

  getAdLikesCount(adId: number) {
    return this.http.get(this.baseUrl + 'likes/count/ad/' + adId);
  }

  getUserFavorites(userId: number) {
    return this.http.get(this.baseUrl + 'ads/user/' + userId + '/favorites');
  }

  removeAdFromFavorites(userId: number, adId: number) { // from favorites
    return this.http.delete(this.baseUrl + 'likes/remove/user/' + userId + '/ad/' + adId);
  }

  setMainPhoto(adId: number, id: number) {
    return this.http.post(this.baseUrl + 'ads/' + adId + '/photos/' + id + '/SetMain', {});
  }

  deletePhoto(adId: number, id: number) {
    return this.http.delete(this.baseUrl + 'ads/' + adId + '/photos/' + id);
  }
}
