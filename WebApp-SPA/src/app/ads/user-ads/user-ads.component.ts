import { Component, OnInit } from '@angular/core';
import { Router, RouterLinkActive, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-user-ads',
  templateUrl: './user-ads.component.html',
  styleUrls: ['./user-ads.component.css']
})
export class UserAdsComponent implements OnInit {
  user: User;

  constructor(private userService: UserService, private route: ActivatedRoute,
              private router: Router, private alertify: AlertifyService) { }

  ngOnInit() {
    this.getUser();
  }

  getUser() {
    // tslint:disable-next-line: no-string-literal
    this.userService.getUser(+this.route.snapshot.params['id']).subscribe((user: User) => {
        this.user = user;
      },
      error => {
        this.alertify.error(error);
        this.router.navigate(['']);
      });
  }

  logConsole() {
    console.log(this.user.ads);
  }
    /*
  loadAd() {
    // tslint:disable-next-line: no-string-literal
    this.adService.getAd(+this.route.snapshot.params['id']).subscribe((ad: Ad) => {
      this.ad = ad;
    }, error => {
      console.log(error);
      // this.alertify.error(error);
    });
  }
  */
}
