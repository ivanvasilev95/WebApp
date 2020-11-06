import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-user-ads',
  templateUrl: './user-ads.component.html',
  styleUrls: ['./user-ads.component.css']
})
export class UserAdsComponent implements OnInit {
  user: User;

  constructor(private userService: UserService, private authService: AuthService,
              private alertify: AlertifyService, private route: ActivatedRoute,
              private router: Router) { }

  ngOnInit() {
    this.getUserWithAds();
  }

  getUserWithAds() {
    const userId = +this.route.snapshot.params.id;
    const includeNotApprovedAds = this.includeNotApprovedAds(userId);

    this.userService.getUserWithAds(userId, includeNotApprovedAds).subscribe(user => {
        this.user = user;
      },
      error => {
        this.alertify.error(error);
        this.router.navigate(['']);
      });
  }

  includeNotApprovedAds(userId) {
    if (this.authService.loggedIn()) {
      return +this.authService.decodedToken.nameid === userId || this.authService.roleMatch(['Admin', 'Moderator']);
    }

    return false;
  }

  comparator(a, b) {
    if (a.isApproved === b.isApproved) {
       return a.dateAdded < b.dateAdded ? 1 : -1;
    }
    return a.isApproved > b.isApproved ? 1 : -1;
 }
}
