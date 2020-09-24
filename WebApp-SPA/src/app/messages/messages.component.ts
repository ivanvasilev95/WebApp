import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { MessageService } from '../_services/message.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  messageContainer = 'Unread';

  constructor(private messageService: MessageService,
              private route: ActivatedRoute,
              private alertify: AlertifyService,
              private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.messages = data.messages.result;
      this.pagination = data.messages.pagination;
    });
  }

  isNotRead(message: Message) {
    return !message.isRead && message.senderId !== +this.authService.decodedToken.nameid;
  }

  senderDeletedIt(message: Message) {
    return message.senderDeleted && message.recipientId === +this.authService.decodedToken.nameid;
  }

  senderIsLoggedUser(message: Message) {
    return message.senderId === +this.authService.decodedToken.nameid;
  }

  recipientOfTheAd(message: Message): number {
    if (message.senderId === +this.authService.decodedToken.nameid) {
      return message.recipientId;
    } else {
      return message.senderId;
    }
  }

  loadMessages(messageFilter: string = null, returnToFirstPage: boolean = false) {
    if (returnToFirstPage) {
      this.pagination.currentPage = 1;
    }

    if (messageFilter !== null && messageFilter !== undefined) {
      this.messageContainer = messageFilter;
    }

    this.messageService.getMessages(this.pagination.currentPage,
      this.pagination.itemsPerPage, this.messageContainer)
      .subscribe((res: PaginatedResult<Message[]>) => {
        this.messages = res.result;
        this.pagination = res.pagination;
      }, error => {
        this.alertify.error(error);
      });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }

  deleteMessage(id: number) {
    this.alertify.confirm('Сигурни ли сте, че искате да изтриете това съобщение', () => {
      this.messageService.deleteMessage(id).subscribe(() => {
        const messageIndex = this.messages.findIndex(m => m.id === id);
        if (this.messages[messageIndex].recipientId === +this.authService.decodedToken.nameid
         && this.messages[messageIndex].isRead === false) {
          MessageService.unreadMessagesCount--;
        }
        this.loadMessages();
        this.alertify.success('Съобщението беше изтрито успешно');
      }, error => {
        this.alertify.error(error);
      });
    });
  }
}
