import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { MessageService } from '../_services/message.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  messageContainer: 'Unread';

  constructor(private messageService: MessageService, private userService: UserService,
              private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.messages = data.messages.result;
      this.pagination = data.messages.pagination;
    });
  }

  loadMessages() {
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
        if (this.messages[messageIndex].isRead === false) {
          this.userService.unreadMessagesCount--;
        }
        this.messages.splice(messageIndex, 1);
        this.alertify.success('Съобщението беше изтрито успешно');
      }, error => {
        this.alertify.error(error);
      });
    });
  }
}
