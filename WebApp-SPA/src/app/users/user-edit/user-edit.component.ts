import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  user: User;
  @ViewChild('editForm', {static: false}) editForm: NgForm;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute, private alertify: AlertifyService,
              private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data.user;
    });
  }

  updateUser() {
    this.checkUserProperties();

    const userId = this.authService.decodedToken.nameid;
    const email = this.user.email;

    // is not admin and email filed is empty
    if ((email === null || email === undefined) &&  userId !== 1) {
      this.alertify.error('Полето имейл не може да бъде празно');
      return;
    }

    // email is not valid
    if (email !== null && email !== undefined && !this.validateEmail(email)) {
      this.alertify.error('Имейлът адресът не е валиден');
      return;
    }

    this.userService.updateUser(userId, this.user).subscribe(() => {
      this.alertify.success('Профилът е редактиран успешно');
      this.editForm.reset(this.user);
    }, error => {
      this.alertify.error(error);
    });
  }

  checkUserProperties() {
    if (this.user.fullName.trim() === '') { this.user.fullName = null; }
    if (this.user.address.trim() === '') { this.user.address = null; }
    if (this.user.email !== null && this.user.email !== undefined) {
      if (this.user.email.trim() === '') {
        this.user.email = null;
      } else {
        this.user.email = this.user.email.toLowerCase().trim();
      }
    }
  }

  validateEmail(email) {
    const re = /\S+@\S+\.\S+/;
    return re.test(email);
  }
}
