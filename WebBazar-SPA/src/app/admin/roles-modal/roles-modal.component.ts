import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
  @Output() updateSelectedRoles = new EventEmitter();
  user: User;
  roles: any[];
  initialRoles: string;
  noChangesHasBeenMade = true;

  constructor(public bsModalRef: BsModalRef, private authService: AuthService) {}

  ngOnInit() {
    this.initialRoles = JSON.stringify(this.roles);
  }

  updateRoles() {
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }

  changeRole(role) {
    role.checked = !role.checked;
    this.uncheckUnnecessaryRoles(role);
    this.checkForChanges();
  }

  private uncheckUnnecessaryRoles(role) {
    if (role.value === 'Admin' && role.checked === true) {
      this.roles.filter(r => r.value === 'Moderator')[0].checked = false;
    }
    if (role.value === 'Moderator' && role.checked === true) {
      this.roles.filter(r => r.value === 'Admin')[0].checked = false;
      this.roles.filter(r => r.value === 'Member')[0].checked = false;
    }
    if (role.value === 'Member' && role.checked === true) {
      this.roles.filter(r => r.value === 'Moderator')[0].checked = false;
    }
  }

  checkForChanges() {
    if (this.initialRoles === JSON.stringify(this.roles)) {
      this.noChangesHasBeenMade = true;
    } else {
      this.noChangesHasBeenMade = false;
    }
  }

  noCheckedRoles() {
    return !this.roles.some(role => role.checked === true);
  }

  areMyRoles() {
    return this.user.id === +this.authService.decodedToken.nameid;
  }

  areAdminRoles() {
    return this.user.userName === 'admin';
  }
}
