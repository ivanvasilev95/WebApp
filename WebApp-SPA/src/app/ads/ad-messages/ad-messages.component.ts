import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Message } from 'src/app/_models/message';
import { tap } from 'rxjs/operators';
import { MessageService } from 'src/app/_services/message.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-ad-messages',
  templateUrl: './ad-messages.component.html',
  styleUrls: ['./ad-messages.component.css']
})
export class AdMessagesComponent implements OnInit {
  @Input() recipientId: number;
  @Input() adId: number;
  messages: Message[];
  newMessage: any = {};

  constructor(private messageService: MessageService, private userService: UserService,
              private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages() {
    const currentUserId = +this.authService.decodedToken.nameid;
    this.messageService.getMessageThread(this.adId, this.recipientId)
      .pipe(
        tap(messages => {
            for (const message of messages) {
              if (message.isRead === false && message.recipientId === currentUserId) {
                this.messageService.markMessageAsRead(message.id);
                this.userService.unreadMessagesCount--;
              }
            }
        })
      )
      .subscribe(result => {
        this.messages = result;
      }, error => {
        this.alertify.error(error);
      });
  }

  sendMessage() {
    const senderId = +this.authService.decodedToken.nameid;

    this.newMessage.adId = this.adId;
    this.newMessage.senderId = senderId;

    this.newMessage.recipientId = this.recipientId;

    this.messageService.sendMessage(this.newMessage).subscribe((message: Message) => {
      this.messages.push(message);
      this.newMessage.content = '';
    }, error => {
      this.alertify.error(error);
    });
  }
}
