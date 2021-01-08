import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginatedResult } from '../_models/pagination';
import { Message } from '../_models/message';
import { catchError, map, tap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  public static unreadMessagesCount: number;
  baseUrl = environment.apiUrl + 'messages/';

  constructor(private http: HttpClient) {}

  getMessages(currentPage: number, itemsPerPage: number, messageFilter: string) {
    const params = this.addHttpParamsForMessages(currentPage, itemsPerPage, messageFilter);

    return this.http.get<Message[]>(this.baseUrl, {observe: 'response', params})
      .pipe(
        map(response => {
          const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();

          paginatedResult.result = response.body;

          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }

          return paginatedResult;
        })
      );
  }

  private addHttpParamsForMessages(currentPage: number, itemsPerPage: number, messageFilter: string): HttpParams {
    let params = new HttpParams();

    params = params.append('pageNumber', currentPage.toString());
    params = params.append('pageSize', itemsPerPage.toString());
    params = params.append('messageFilter', messageFilter);

    return params;
  }

  getMessageThread(adId: number, recipientId: number) {
    let params = new HttpParams();

    params = params.append('adId', adId.toString());
    params = params.append('recipientId', recipientId.toString());

    return this.http.get<Message[]>(this.baseUrl + 'thread', {params});
  }

  getUnreadMessagesCount() {
    return this.http.get(this.baseUrl + 'unreadCount')
      .pipe(
        tap((count: number) => { MessageService.unreadMessagesCount = count; }),
        catchError(error => {
          MessageService.unreadMessagesCount = -1;
          return of(null);
        })
      );
  }

  sendMessage(message: Message): Observable<Date> {
    return this.http.post<Date>(this.baseUrl, message);
  }

  markMessageAsRead(id: number) {
    return this.http.put(this.baseUrl + id + '/markAsRead', {});
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + id);
  }
}
