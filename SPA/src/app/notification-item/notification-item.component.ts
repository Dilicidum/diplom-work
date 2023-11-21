import { Component } from '@angular/core';
import { Input } from '@angular/core';
import { Notification } from '../models/notification';
import { Router } from '@angular/router';
@Component({
  selector: 'app-notification-item',
  templateUrl: './notification-item.component.html',
  styleUrls: ['./notification-item.component.css'],
})
export class NotificationItemComponent {
  @Input() notification: Notification;

  constructor(private router: Router) {}

  navigateToTask() {
    this.router.navigateByUrl('', { skipLocationChange: true }).then(() => {
      this.router.navigate([
        '/tasks/' + this.notification.taskId + '/' + this.notification.userId,
      ]);
    });
  }
}
