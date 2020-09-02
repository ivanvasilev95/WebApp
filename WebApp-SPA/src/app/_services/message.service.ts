import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginatedResult } from '../_models/pagination';
import { Message } from '../_models/message';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getMessages(page?: number, itemsPerPage?: number, messageContainer?: string) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();
    let params = new HttpParams();
    params = params.append('MessageContainer', messageContainer);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http.get<Message[]>(this.baseUrl + 'messages', {observe: 'response', params})
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

  getMessageThread(adId: number, recipientId: number) {
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + adId + '/' + recipientId);
  }

  sendMessage(message: Message) {
    return this.http.post(this.baseUrl + 'messages', message);
  }

  deleteMessage(id: number) {
    return this.http.post(this.baseUrl + 'messages/' + id, {});
  }

  markMessageAsRead(id: number) {
    this.http.post(this.baseUrl + 'messages/' + id + '/read', {}).subscribe();
  }

  getUnreadMessagesCount() {
    return this.http.get(this.baseUrl + 'messages/user/unread');
  }
}
