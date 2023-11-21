import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Notification } from '../models/notification';
@Injectable({
  providedIn: 'root',
})
export class NotificationsService {
  baseUrl: string = 'http://localhost:5292/Users';
  constructor(private http: HttpClient) {}

  getNotifications(userId: string): Observable<Notification[]> {
    return this.http.get<Notification[]>(
      this.baseUrl + '/' + userId + '/Notifications'
    );
  }
}
