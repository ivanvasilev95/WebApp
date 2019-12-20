import { Component, OnInit, Input } from '@angular/core';

import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Message } from 'src/app/_models/message';
import { Ad } from 'src/app/_models/ad';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-ad-messages',
  templateUrl: './ad-messages.component.html',
  styleUrls: ['./ad-messages.component.css']
})
export class AdMessagesComponent implements OnInit {
  @Input() recipientId: number;
  @Input() adId: number;
  // @Input() ad: Ad;
  messages: Message[];
  newMessage: any = {};

  constructor(private userService: UserService, private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages() {
    const currentUserId = +this.authService.decodedToken.nameid;
    this.userService.getMessageThread(this.adId, this.recipientId)
      .pipe(
        tap(messages => {
          // tslint:disable-next-line: prefer-for-of
          for (let i = 0; i < messages.length; i++) {
            if (messages[i].isRead === false && messages[i].recipientId === currentUserId) {
              this.userService.markAsRead(messages[i].id);
              this.authService.unreadMsgCnt--;
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

    this.newMessage.adId = this.adId; // this.ad.id;
    this.newMessage.senderId = senderId;

    // if (this.recipientId !== senderId) {
    this.newMessage.recipientId = this.recipientId;
    // } else {
      // this.newMessage.recipientId = this.ad.userId;
    // }

    this.userService.sendMessage(this.newMessage).subscribe((message: Message) => {
      this.messages.push(message);
      this.newMessage.content = '';
    }, error => {
      this.alertify.error(error);
    });
  }
}
