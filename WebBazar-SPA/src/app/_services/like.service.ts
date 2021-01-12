import { HttpParams } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LikeService {
  baseUrl = environment.apiUrl + 'likes/';

  constructor(private http: HttpClient) { }

  likeAd(adId: number) {
    return this.http.post(this.baseUrl + 'likeAd/' + adId, {});
  }

  unlikeAd(adId: number) {
    return this.http.delete(this.baseUrl + 'unlikeAd/' + adId);
  }
}
