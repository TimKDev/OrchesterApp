import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { Observable, NEVER, catchError } from 'rxjs';
import { NotificationApiService } from 'src/app/core/services/notification-api.service';
import { NotificationDto } from 'src/app/core/interfaces/notification-dto.interface';
import { NotificationUrgency } from 'src/app/core/services/notification-urgency.enum';
import { NotificationService } from 'src/app/core/services/notification.service';
import { NotificationType } from 'src/app/core/services/notification-type.enum';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss'],
})
export class NotificationsComponent implements OnInit {

  notifications: NotificationDto[] = [];
  isLoading = true;

  constructor(
    private location: Location,
    private router: Router,
    private notificationApiService: NotificationApiService,
    private notificationService : NotificationService
  ) {}

  ngOnInit() {
    this.loadNotifications();
  }

  private loadNotifications() {
    this.isLoading = true;

    this.notificationApiService.getNotificationsForUser().pipe(
      catchError(() => {
        this.isLoading = false;
        return NEVER;
      })
    ).subscribe((response) => {
      this.notifications = response.notifications;
      this.isLoading = false;
      this.updateUnreadCount(response.notifications.filter(n => !n.isRead).length);
    });
  }

  getNotificationIcon(type: NotificationType): string {
    switch (type) {
      case NotificationType.Info:
        return 'information-circle-outline';
      case NotificationType.Warning:
        return 'warning-outline';
      case NotificationType.Error:
        return 'alert-circle-outline';
      case NotificationType.Success:
        return 'checkmark-circle-outline';
      default:
        return 'notifications-outline';
    }
  }

  getNotificationColor(urgency: NotificationUrgency): string {
    switch (urgency) {
      case NotificationUrgency.Low:
        return 'medium';
      case NotificationUrgency.Medium:
        return 'primary';
      case NotificationUrgency.High:
        return 'warning';
      case NotificationUrgency.Critical:
        return 'danger';
      default:
        return 'medium';
    }
  }

  getIconColor(type: NotificationType, urgency: NotificationUrgency): string {
    switch (type) {
      case NotificationType.Success:
        return 'success';
      case NotificationType.Error:
        return 'danger';
      default:
        return this.getNotificationColor(urgency);
    }
  }

  markAsRead(notification: NotificationDto) {
    if (!notification.isRead) {
      notification.isRead = true;
      this.updateUnreadCount(this.notificationService.numberOfUnread.value - 1);
      //Hier sollte noch der zweite Endpunkt aufgerufen werden
    }
  }

  private updateUnreadCount(unreadNotification: number) {
    this.notificationService.numberOfUnread.next(unreadNotification);
  }

  goBack() {
    this.location.back();
  }

  navigateToTermin(terminId: string) {
    if (terminId) {
      this.router.navigate(['/tabs/termin/details', terminId, 'default']);
    }
  }
}
