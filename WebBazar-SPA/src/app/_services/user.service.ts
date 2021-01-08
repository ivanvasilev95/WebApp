import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpParams } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + 'users/';

  constructor(private http: HttpClient) {}

  getUserWithAds(id: number, includeNotApprovedAds: boolean): Observable<User> {

    return this.http.get<User>(this.baseUrl + id, {params: this.createQueryString(includeNotApprovedAds)});
  }

  private createQueryString(includeNotApprovedAds: boolean) {
    return new HttpParams().append('includeNotApprovedAds', includeNotApprovedAds.toString());
  }

  getUserForEdit(id: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + id + '/details');
  }

  editUser(id: number, user: User) {
    return this.http.put(this.baseUrl + id, user);
  }
}
