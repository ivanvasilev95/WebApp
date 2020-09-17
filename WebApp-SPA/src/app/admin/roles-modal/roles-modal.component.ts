import { Component, EventEmitter, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent {
  @Output() updateSelectedRoles = new EventEmitter();
  user: User;
  roles: any[];

  constructor(public bsModalRef: BsModalRef, private authService: AuthService) {}

  updateRoles() {
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }

  changeRole(role) {
    role.checked = !role.checked;
    if (role.value === 'Admin') {
      // ...
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
