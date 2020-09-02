import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Ad } from '../_models/ad';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAds(page?: number, itemsPerPage?: number, userParams?: any): Observable<PaginatedResult<Ad[]>> {
    const paginatedResult: PaginatedResult<Ad[]> = new PaginatedResult<Ad[]>();

    const params = this.addHttpParams(page, itemsPerPage, userParams);

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

  private addHttpParams(page?: number, itemsPerPage?: number, userParams?: any): HttpParams {
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if (userParams != null) {
      params = params.append('searchText', userParams.searchText);
      params = params.append('categoryId', userParams.categoryId);
      params = params.append('sortCriteria', userParams.sortCriteria);
    }

    return params;
  }

  getAd(id: number): Observable<Ad> {
    return this.http.get<Ad>(this.baseUrl + 'ads/' + id);
  }

  getUserAds(): Observable<Ad[]> {
    return this.http.get<Ad[]>(this.baseUrl + 'ads/user');
  }

  deleteAd(id: number) {
    return this.http.delete(this.baseUrl + 'ads/' + id);
  }

  updateAd(id: number, ad: Ad) {
    return this.http.put(this.baseUrl + 'ads/' + id, ad);
  }

  createAd(ad: Ad) {
    return this.http.post(this.baseUrl + 'ads', ad);
  }

  addAdToLiked(userId: number, adId: number) {
    return this.http.post(this.baseUrl + 'likes/user/' + userId + '/ad/' + adId, {});
  }

  getAdLikesCount(adId: number) {
    return this.http.get(this.baseUrl + 'likes/count/ad/' + adId);
  }

  getUserLikedAds(userId: number) {
    return this.http.get(this.baseUrl + 'ads/user/' + userId + '/liked');
  }

  removeAdFromLiked(userId: number, adId: number) {
    return this.http.delete(this.baseUrl + 'likes/remove/user/' + userId + '/ad/' + adId);
  }
}
