import { Component, OnInit } from '@angular/core';
import { NotificationsService } from '../services/notifications.service';
import { Observable } from 'rxjs';
import { Notification } from '../models/notification';
@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css'],
})
export class NotificationsComponent implements OnInit {
  notifications$: Observable<Notification[]>;
  constructor(private notificationsService: NotificationsService) {}

  showNotifications: boolean = false;

  ngOnInit() {
    let userId = localStorage.getItem('userId');

    this.notifications$ = this.notificationsService.getNotifications(userId);
  }

  showNotification() {
    this.showNotifications = !this.showNotifications;
  }
}
