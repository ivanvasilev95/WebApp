import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { User } from '../_models/user';
import { Message } from '../_models/message';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

/*
const httpOptions = {
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + localStorage.getItem('token')
  })
};
*/

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id/*, httpOptions*/);
  }

  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }

  getMessages(/*messageContainer?*//*id: number, page?, itemsPerPage?, messageContainer?*/) {
    // tslint:disable-next-line: prefer-const
    // let result: Message[];
    // tslint:disable-next-line: prefer-const
    // let params = new HttpParams();
    // params.append('MessageContainer', messageContainer);

    return this.http.get<Message[]>(this.baseUrl + 'messages'/*, {observe: 'response', params}*/);
      /*.pipe(
        map(response => {

        })
      );*/
  }

  getMessageThread(adId: number, recipientId: number) {
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + adId + '/' + recipientId);
  }

  sendMessage(message: Message) { // this.recipientId
    return this.http.post(this.baseUrl + 'messages', message);
  }

  deleteMessage(id: number) {
    return this.http.post(this.baseUrl + 'messages/' + id, {});
  }

  markAsRead(id: number) {
    this.http.post(this.baseUrl + 'messages/' + id + '/read', {}).subscribe();
  }
}
