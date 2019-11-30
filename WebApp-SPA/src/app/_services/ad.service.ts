import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Ad } from '../_models/ad';
import { Observable } from 'rxjs';

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
}
