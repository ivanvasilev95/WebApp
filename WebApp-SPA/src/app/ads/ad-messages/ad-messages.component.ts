import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Message } from 'src/app/_models/message';
import { tap } from 'rxjs/operators';
import { MessageService } from 'src/app/_services/message.service';

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

  constructor(private messageService: MessageService,
              private authService: AuthService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.newMessage.content = '';
  }

  loadMessages() {
    const currentUserId = +this.authService.decodedToken.nameid;
    this.messageService.getMessageThread(this.adId, this.recipientId)
      .pipe(
        tap(messages => {
            for (const message of messages) {
              if (message.isRead === false && message.recipientId === currentUserId /* && message.senderDeleted === false */) {
                this.messageService.markMessageAsRead(message.id);
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
    const messageLength = this.newMessage.content.length;
    if (isNaN(messageLength) || messageLength < 5) {
      this.alertify.error('Съобщението трябва да бъде поне 5 символа');
      return;
    }

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
