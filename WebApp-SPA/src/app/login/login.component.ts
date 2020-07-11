import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  model: any = {};
  loginForm: FormGroup;

  constructor(public authService: AuthService, private alertify: AlertifyService, private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.createLoginForm();
  }

  createLoginForm() {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  login() {
    if (this.loginForm.valid) {
      this.model = Object.assign({}, this.loginForm.value);
      this.authService.login(this.model).subscribe(next => {
        this.alertify.success('Успешен вход');
        if (this.authService.roleMatch(['Admin', 'Moderator'])) {
          this.router.navigate(['/admin']);
        } else {
          this.router.navigate(['/user/ads']);
        }
      }, error => {
        this.alertify.error(error); // 'Невалидно потребителско име или парола'
      });
    }
  }
}
