import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Roles } from '../models/roles';
import { UserRegistrationModel } from '../models/userRegistrationModel';
import { Router } from '@angular/router';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  registrationForm: FormGroup;
  roles = Object.values(Roles);
  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registrationForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required]],
      role: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {}

  onSubmit() {
    if (this.registrationForm.invalid) {
      console.log('Invalid form');
      return;
    }

    let model = {
      email: this.registrationForm.value.email,
      username: this.registrationForm.value.username,
      password: this.registrationForm.value.password,
      role: this.registrationForm.value.role,
    } as UserRegistrationModel;
    console.log('model = ', model);
    this.authService.register(model).subscribe({
      next: () => {
        this.registrationForm.reset();
      },
      error: (err) => {
        console.log(err);
      },
    });
    this.router.navigate(['/login']);
    this.registrationForm.reset();
    this.registrationForm.disable();
  }
}
