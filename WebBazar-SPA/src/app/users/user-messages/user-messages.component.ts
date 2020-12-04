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
        if (this.messages.length - 1 === 0 && this.pagination.currentPage > 1) {
          this.pagination.currentPage = this.pagination.currentPage - 1;
        }
        this.loadMessages();
        this.alertify.success('Съобщението беше изтрито успешно');
      }, error => {
        this.alertify.error(error);
      });
    });
  }

  getMessageType(type) {
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

  senderIsLoggedInUser(message: Message) {
    return message.senderId === this.getLoggedInUserId();
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
