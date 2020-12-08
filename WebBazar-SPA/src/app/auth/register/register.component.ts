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
      userName: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]],
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(12)]],
      confirmPassword: ['', Validators.required]
    }, {validator: [this.userNameValidator, this.fullNameValidator, this.passwordValidator, this.passwordMatchValidator]});
  }

  userNameValidator(g: FormGroup) {
    const userName = g.get('userName').value === null ? '' : g.get('userName').value;
    const regex = /\s/g; // checks if string contains whitespaces

    if (regex.test(userName)) {
      return {userNameMismatch: true};
    }

    return null;
  }

  fullNameValidator(g: FormGroup) {
    const fullName = g.get('fullName').value === null ? '' : g.get('fullName').value.trimEnd();

    if (fullName.length < 2 || fullName.length > 15) {
      return {fullNameMismatch: true};
    }

    return null;
  }

  passwordValidator(g: FormGroup) {
    const password = g.get('password').value === null ? '' : g.get('password').value;
    const regex = /\s/g; // checks if string contains whitespaces

    if (regex.test(password)) {
      return {passwordMismatch: true};
    }

    return null;
  }

  passwordMatchValidator(g: FormGroup) {
    const password = g.get('password').value === null ? '' : g.get('password').value;
    const confirmPassword = g.get('confirmPassword').value === null ? '' : g.get('confirmPassword').value;

    return password === confirmPassword ? null : {confirmPasswordMismatch: true};
  }

  register() {
    if (this.registerForm.valid) {
      const user = Object.assign({}, this.registerForm.value);
      user.fullName = user.fullName.trimEnd();
      delete user.confirmPassword;

      this.authService.register(user).subscribe(() => {
        this.alertify.success('Регистрацията е направена успешно');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.authService.login({userName: user.userName, password: user.password}).subscribe(() => {
          this.router.navigate(['']);
        });
      });
    }
  }

  trimUserName(event) {
    this.registerForm.controls.userName.setValue(event.target.value.toLowerCase().trim());
  }

  trimFullName(event) {
    // removes whitespaces at the beginning and replaces middle whitespaces with a single whitespace;
    this.registerForm.controls.fullName.setValue(event.target.value.trimStart().replace(/\s+/g, ' '));
  }

  trimEmail(event) {
    this.registerForm.controls.email.setValue(event.target.value.toLowerCase().trim());
  }

  trimPassword(event) {
    this.registerForm.controls.password.setValue(event.target.value.trim());
  }

  trimConfirmPassword(event) {
    this.registerForm.controls.confirmPassword.setValue(event.target.value.trim());
  }
}
