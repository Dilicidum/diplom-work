import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserLoginModel } from '../models/userLoginModel';
import { tap } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { UserRegistrationModel } from '../models/userRegistrationModel';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  loginUrl: string = 'http://localhost:5292/Authentication/Login';
  registrationUrl: string = 'http://localhost:5292/Authentication/Register';
  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) {}

  login(data: UserLoginModel) {
    return this.http.post(this.loginUrl, data).pipe(
      tap((res: any) => {
        console.log('res = ', res);

        localStorage.setItem('token', res.token);

        this.isLoggedIn$.next(true);
      })
    );
  }

  register(data: UserRegistrationModel) {
    return this.http.post(this.registrationUrl, data);
  }

  logout() {
    this.isLoggedIn$.next(false);
    console.log('remove token');
    localStorage.removeItem('token');
  }

  isAuthenticated() {
    if (localStorage.getItem('token') && !this.jwtHelper.isTokenExpired()) {
      return true;
    } else {
      return false;
    }
  }

  isLoggedIn$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    this.isAuthenticated()
  );

  token: string = localStorage.getItem('token') || '';
}
