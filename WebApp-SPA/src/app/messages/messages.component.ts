import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { UserService } from '../_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { Ad } from '../_models/ad';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  messageContainer: 'Unread';

  constructor(private userService: UserService, private authService: AuthService,
              private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.messages = data['messages'];
    });
  }

  loadMessages() {
  }

  deleteMessage(id: number) {
    this.alertify.confirm('Сигурни ли сте, че искате да изтриете това съобщение', () => {
      this.userService.deleteMessage(id).subscribe(() => {
        const msgIndex = this.messages.findIndex(m => m.id === id);
        if (this.messages[msgIndex].isRead === false) {
          this.authService.unreadMsgCnt--;
        }
        this.messages.splice(msgIndex, 1);
        this.alertify.success('Съобщението беше изтрито успешно');
      }, error => {
        this.alertify.error('Неуспешно изтриване');
      });
    });
  }

  /*
  deleteMessage(id: number) {
    this.alertify.confirm('Are you sure you want to delete this message', () => {
      this.userService.deleteMessage(id).subscribe(() => {
        this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
        this.alertify.success('Message has been deleted');
      }, error => {
        this.alertify.error('Failed to delete the message');
      });
    });
  }
  */
}
