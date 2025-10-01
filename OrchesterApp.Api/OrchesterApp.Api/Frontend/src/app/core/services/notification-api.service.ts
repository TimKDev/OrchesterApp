import { Injectable } from '@angular/core';
import { AuthHttpClientService } from './auth-http-client.service';
import { Observable } from 'rxjs';
import { NotificationsForUserResponse } from './notifications-for-user-response.interface';
import { AcknowledgeNotificationRequest } from '../interfaces/acknowledge-notification-request.interface';

@Injectable({
  providedIn: 'root'
})
export class NotificationApiService {

  constructor(private http: AuthHttpClientService) { }

  public getNotificationsForUser(): Observable<NotificationsForUserResponse> {
    return this.http.get<NotificationsForUserResponse>('api/notifications/user/');
  }

  public acknowledgeNotifications(request: AcknowledgeNotificationRequest): Observable<void> {
    return this.http.post<void>('api/notifications/acknowledge/', request);
  }
}
