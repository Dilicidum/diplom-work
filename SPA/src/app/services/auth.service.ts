import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserLoginModel } from '../models/userLoginModel';
import { map, tap } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { UserRegistrationModel } from '../models/userRegistrationModel';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _token = '';
  loginUrl: string = 'http://localhost:5292/Authentication/Login';
  registrationUrl: string = 'http://localhost:5292/Authentication/Register';
  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) {}

  login(data: UserLoginModel) {
    return this.http.post(this.loginUrl, data).pipe(
      tap((res: any) => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('userId', res.userId);
        this.isLoggedIn$.next(true);
      })
    );
  }

  register(data: UserRegistrationModel) {
    return this.http.post(this.registrationUrl, data);
  }

  logout() {
    this.isLoggedIn$.next(false);
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

  get token() {
    return localStorage.getItem('token');
  }

  set token(value: any) {
    this._token = value;
  }
}
