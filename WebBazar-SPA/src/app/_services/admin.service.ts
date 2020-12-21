import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl + 'admin/';

  constructor(private http: HttpClient) { }

  getUsersWithRoles() {
    return this.http.get(this.baseUrl + 'usersWithRoles');
  }

  getRoles() {
    return this.http.get<string[]>(this.baseUrl + 'roles');
  }

  updateUserRoles(userName: string, roles: {}) {
    return this.http.put(this.baseUrl + 'editRoles/' + userName, roles);
  }

  getAdsForApproval() {
    return this.http.get(this.baseUrl + 'adsForApproval');
  }

  approveAd(id) {
    return this.http.put(this.baseUrl + 'approveAd/' + id, {});
  }

  rejectAd(id) {
    return this.http.delete(this.baseUrl + 'rejectAd/' + id);
  }
}
