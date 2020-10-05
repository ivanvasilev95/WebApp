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

    this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
      this.alertify.success('Профилът е редактиран успешно');
      this.editForm.reset(this.user);
    }, error => {
      this.alertify.error(error);
    });
  }

  checkUserProperties() {
    if (this.user.fullName.trim() === '') { this.user.fullName = null; }
    if (this.user.address.trim() === '') { this.user.address = null; }
    if (this.user.email.trim() === '') {
      this.user.email = null;
    } else  if (this.user.email !== null && this.user.email !== undefined) {
      this.user.email = this.user.email.toLowerCase().trim();
    }
  }
}
