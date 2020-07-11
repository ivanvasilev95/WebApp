import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsersWithRoles() {
    return this.http.get(this.baseUrl + 'admin/usersWithRoles');
  }

  getRoles() {
    return this.http.get<string[]>(this.baseUrl + 'admin/roles');
  }

  updateUserRoles(user: User, roles: {}) {
    return this.http.post(this.baseUrl + 'admin/editRoles/' + user.userName, roles);
  }

  getAdsForApproval() {
    return this.http.get(this.baseUrl + 'admin/adsForModeration');
  }

  approveAd(adId) {
    return this.http.post(this.baseUrl + 'admin/approveAd/' + adId, {});
  }

  rejectAd(adId) {
    return this.http.post(this.baseUrl + 'admin/rejectAd/' + adId, {});
  }
}
