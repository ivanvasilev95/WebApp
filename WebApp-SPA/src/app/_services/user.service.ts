import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + 'users/';

  constructor(private http: HttpClient) {}

  getUserWithAds(id: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + id);
  }

  getUserForEdit(id: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + id + '/edit');
  }

  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + id, user);
  }
}
