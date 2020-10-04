import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;

  constructor(private authService: AuthService, private router: Router,
              private alertify: AlertifyService, private fb: FormBuilder) { }

  ngOnInit() {
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      userName: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]],
      fullName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(15)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(12)]],
      confirmPassword: ['', Validators.required]
    }, {validator: this.passwordMatchValidator});
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : {mismatch: true};
  }

  register() {
    if (this.registerForm.valid) {
      const user = Object.assign({}, this.registerForm.value);
      delete user.confirmPassword;
      user.userName = user.userName.toLowerCase().trim();
      user.email = user.email.toLowerCase().trim();

      this.authService.register(user).subscribe(() => {
        this.alertify.success('Регистрацията е направена успешно');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.authService.login(user).subscribe(() => {
          this.router.navigate(['']);
        });
      });
    }
  }
}
