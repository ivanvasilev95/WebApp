import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
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
        this.adminService.updateUserRoles(user.userName, rolesToUpdate).subscribe(() => {
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

    for (const role of this.roles) {
      let isMatch = false;
      for (const userRole of userRoles) {
        if (role.name === userRole) {
          isMatch = true;
          role.checked = true;
          rolesForModal.push(role);
          break;
        }
      }
      if (!isMatch) {
        role.checked = false;
        rolesForModal.push(role);
      }
    }

    return rolesForModal;
  }
}

