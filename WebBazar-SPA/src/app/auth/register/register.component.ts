import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
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
      userName: [''],
      fullName: [''],
      email: ['', [Validators.required, Validators.email]],
      password: [''],
      confirmPassword: ['', Validators.required]
    }, {validator: [this.userNameValidator, this.fullNameValidator, this.passwordValidator, this.passwordMatchValidator]});
  }

  userNameValidator(g: FormGroup) {
    const userName = g.get('userName').value === null ? '' : g.get('userName').value.trim();
    const regex = /\s/g; // checks if string contains whitespaces
    if (regex.test(userName) || userName.length < 4 || userName.length > 10) {
      return {userNameMismatch: true};
    }
    return null;
  }

  fullNameValidator(g: FormGroup) {
    const fullName = g.get('fullName').value === null ? '' : g.get('fullName').value.trim();
    if (fullName.length < 2 || fullName.length > 15) {
      return {fullNameMismatch: true};
    }
    return null;
  }

  passwordValidator(g: FormGroup) {
    const password = g.get('password').value === null ? '' : g.get('password').value.trim();
    const regex = /\s/g; // checks if string contains whitespaces
    if (regex.test(password) || password.length < 6 || password.length > 12) {
      return {passwordMismatch: true};
    }
    return null;
  }

  passwordMatchValidator(g: FormGroup) {
    const password = g.get('password').value === null ? '' : g.get('password').value.trim();
    const confirmPassword = g.get('confirmPassword').value === null ? '' : g.get('confirmPassword').value.trim();

    return password === confirmPassword ? null : {mismatch: true};
  }

  register() {
    if (this.registerForm.valid) {
      const user = Object.assign({}, this.registerForm.value);
      delete user.confirmPassword;
      user.fullName = user.fullName.trim();
      user.password = user.password.trim();
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

  trim(event) {
    event.target.value = event.target.value.trim();
  }
}
