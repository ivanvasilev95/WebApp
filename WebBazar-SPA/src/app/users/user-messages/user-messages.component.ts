import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Message } from 'src/app/_models/message';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-user-messages',
  templateUrl: './user-messages.component.html',
  styleUrls: ['./user-messages.component.css']
})
export class UserMessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  messageFilter = 'Unread';

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

  pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }

  filterMessages(messageFilter: string) {
    if (this.messageFilter === messageFilter) {
      return;
    }

    this.messageFilter = messageFilter;
    this.pagination.itemsPerPage = 5;

    this.loadFromTheBeginning();
  }

  loadFromTheBeginning() {
    if (this.pagination.currentPage > 1) {
      this.pageChanged({page: 1}); // this.pagination.currentPage = 1;
    } else {
      this.loadMessages();
    }
  }

  loadMessages() {
    this.messageService.getMessages(this.pagination.currentPage, this.pagination.itemsPerPage, this.messageFilter)
      .subscribe((res: PaginatedResult<Message[]>) => {
        this.messages = res.result;
        this.pagination = res.pagination;
      }, error => {
        this.alertify.error(error);
      });
  }

  deleteMessage(id: number) {
    this.alertify.confirm('Сигурни ли сте, че искате да изтриете това съобщение', () => {
      this.messageService.deleteMessage(id).subscribe(() => {
        if (this.messages.length - 1 === 0 && this.pagination.currentPage > 1) {
          this.pagination.currentPage = this.pagination.currentPage - 1;
        } else {
          this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
          this.pagination.totalItems = this.pagination.totalItems - 1;
        }
        this.alertify.success('Съобщението беше изтрито успешно');
      }, error => {
        this.alertify.error(error);
      });
    });
  }

  getPaginator() {
    const pageItems = this.pagination.currentPage * this.pagination.itemsPerPage;

    return 'Показване на съобщения ' +
           (1 + (this.pagination.itemsPerPage * (this.pagination.currentPage - 1))) +
           ' - ' +
           (pageItems > this.pagination.totalItems ? this.pagination.totalItems : pageItems) +
           ' от ' +
           this.pagination.totalItems;
  }

  getMessagesType(type) {
    switch (type) {
      case 'Unread': return 'непрочетени';
      case 'Inbox': return 'получени';
      case 'Outbox': return 'изпратени';
    }
  }

  isNotRead(message: Message) {
    return !message.isRead && message.recipientId === this.getLoggedInUserId();
  }

  senderDeletedIt(message: Message) {
    return message.senderDeleted && message.recipientId === this.getLoggedInUserId();
  }

  senderIsLoggedInUser(senderId: number) {
    return senderId === this.getLoggedInUserId();
  }

  getLoggedInUserId() {
    return +this.authService.decodedToken.nameid;
  }

  passDataTroughLocalStorage(message: Message) {
    if (message.senderId === this.getLoggedInUserId()) {
      localStorage.setItem('recipientId', message.recipientId.toString());
    } else {
      localStorage.setItem('recipientId', message.senderId.toString());
    }
    localStorage.setItem('tabNumber', '2');
  }
}
