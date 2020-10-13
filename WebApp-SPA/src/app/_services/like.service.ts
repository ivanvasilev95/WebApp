import { HttpParams } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  baseUrl = environment.apiUrl + 'likes/';

  constructor(private http: HttpClient) { }

  addAdToLiked(adId: number) {
    return this.http.post(this.baseUrl + 'add', {}, {params: this.createQueryString(adId)});
  }

  removeAdFromLiked(adId: number) {
    return this.http.delete(this.baseUrl + 'remove', {params: this.createQueryString(adId)});
  }

  getAdLikesCount(adId: number): Observable<number> {
    return this.http.get<number>(this.baseUrl + 'count', {params: this.createQueryString(adId)});
  }

  private createQueryString(adId: number) {
    return new HttpParams().append('adId', adId.toString());
  }
}
