import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  setMainPhoto(adId: number, id: number) {
    return this.http.post(this.baseUrl + 'photos/' + id + '/setMain/ad/' + adId, {});
  }

  deletePhoto(adId: number, id: number) {
    return this.http.delete(this.baseUrl + 'photos/' + id + '/ad/' + adId);
  }

}
