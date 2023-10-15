import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { UserLoginModel } from '../models/userLoginModel';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      console.log(this.loginForm.value.email);
      return;
    }

    let model = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password,
    } as UserLoginModel;

    this.authService.login(model).subscribe((data) => {
      this.router.navigate(['tasks']);
    });
  }
}
