import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Message } from 'src/app/_models/message';
import { tap } from 'rxjs/operators';
import { MessageService } from 'src/app/_services/message.service';
import { Ad } from 'src/app/_models/ad';

@Component({
  selector: 'app-ad-messages',
  templateUrl: './ad-messages.component.html',
  styleUrls: ['./ad-messages.component.css']
})
export class AdMessagesComponent implements OnInit {
  @Input() recipientId: number;
  @Input() ad: Ad;
  messages: Message[];
  newMessage: any = {};

  constructor(private messageService: MessageService,
              private authService: AuthService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.newMessage.content = '';
  }

  loadMessages() {
    this.messageService.getMessageThread(this.ad.id, this.recipientId)
      .pipe(
        tap(messages => {
          const currentUserId = +this.authService.decodedToken.nameid;
          for (const message of messages) {
            if (message.isRead === false && message.recipientId === currentUserId) {
              this.messageService.markMessageAsRead(message.id).subscribe();
              MessageService.unreadMessagesCount--;
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
    this.newMessage.content = this.newMessage.content.trim();
    if (this.newMessage.content === '') {
      this.alertify.error('Съобщението не може да бъде празно');
      return;
    }

    const messageLength = this.newMessage.content.length;
    if (isNaN(messageLength) || messageLength < 4) {
      this.alertify.error('Съобщението трябва да бъде поне 4 символа');
      return;
    }

    const senderId = +this.authService.decodedToken.nameid;

    this.newMessage.adId = this.ad.id;
    this.newMessage.senderId = senderId;

    this.newMessage.recipientId = this.recipientId;

    this.messageService.sendMessage(this.newMessage).subscribe(message => {
      this.messages.push(message);
      this.newMessage.content = '';
    }, error => {
      this.alertify.error(error);
    });
  }
}
