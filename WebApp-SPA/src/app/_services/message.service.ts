import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginatedResult } from '../_models/pagination';
import { Message } from '../_models/message';
import { map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  public static unreadMessagesCount: number;
  baseUrl = environment.apiUrl + 'messages/';

  constructor(private http: HttpClient) {}

  getMessages(page?: number, itemsPerPage?: number, messageContainer?: string) {
    const params = this.addHttpParamsForMessages(page, itemsPerPage, messageContainer)

    return this.http.get<Message[]>(this.baseUrl, {observe: 'response', params})
      .pipe(
        map(response => {
          const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  private addHttpParamsForMessages(page?: number, itemsPerPage?: number, messageContainer?: string): HttpParams {
    let params = new HttpParams();
    params = params.append('MessageContainer', messageContainer);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return params;
  }

  getMessageThread(adId: number, recipientId: number) {
    let params = new HttpParams();
    params = params.append('adId', adId.toString());
    params = params.append('recipientId', recipientId.toString());

    return this.http.get<Message[]>(this.baseUrl + 'thread', {params});
  }

  sendMessage(message: Message) {
    return this.http.post(this.baseUrl, message);
  }

  deleteMessage(id: number) {
    return this.http.post(this.baseUrl + id, {});
  }

  markMessageAsRead(id: number) {
    return this.http.post(this.baseUrl + id + '/read', {});
  }

  getUnreadMessagesCount() {
    return this.http.get(this.baseUrl + 'user/unread')
      .pipe(
        tap((count: number) => { MessageService.unreadMessagesCount = count; })
      );
  }
}
