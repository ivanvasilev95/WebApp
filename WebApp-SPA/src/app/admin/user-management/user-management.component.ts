import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { RolesModalComponent } from '../roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: User[];
  roles: any[] = [];
  bsModalRef: BsModalRef;

  constructor(private adminService: AdminService, private alertify: AlertifyService,
              private modalService: BsModalService) { }

  ngOnInit() {
    this.getUsersWithRoles();
    this.getRoles();
  }

  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe((users: User[]) => {
      this.users = users;
    }, error => {
      this.alertify.error(error);
    });
  }

  getRoles() {
    this.adminService.getRoles().subscribe(result => {
      result.forEach(role => this.roles.push({name: role, value: role}));
    }, error => {
      this.alertify.error(error);
    });
  }

  editRolesModal(user: User) {
    const initialState = {
        user,
        roles: this.getRolesArray(user)
    };

    this.bsModalRef = this.modalService.show(RolesModalComponent, {initialState});

    this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
      const rolesToUpdate = {
        roleNames: [...values.filter(el => el.checked === true).map(el => el.name)]
      };
      if (rolesToUpdate) {
        this.adminService.updateUserRoles(user, rolesToUpdate).subscribe(() => {
          user.roles = [...rolesToUpdate.roleNames];
        }, error => {
          this.alertify.error(error);
        });
      }
    });
  }

  private getRolesArray(user) {
    const rolesForModal = [];
    const userRoles = user.roles;
    /*
    const roles: any[] = [
      {name: 'Admin', value: 'Admin'},
      {name: 'Moderator', value: 'Moderator'},
      {name: 'Member', value: 'Member'},
      {name: 'VIP', value: 'VIP'}
    ];
    */

    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < this.roles.length; i++) {
      let isMatch = false;
      // tslint:disable-next-line: prefer-for-of
      for (let j = 0; j < userRoles.length; j++) {
        if (this.roles[i].name === userRoles[j]) {
          isMatch = true;
          this.roles[i].checked = true;
          rolesForModal.push(this.roles[i]);
          break;
        }
      }
      if (!isMatch) {
        this.roles[i].checked = false;
        rolesForModal.push(this.roles[i]);
      }
    }

    return rolesForModal;
  }
}

