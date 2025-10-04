import { Component, NgZone, OnDestroy, OnInit } from '@angular/core';
import { Platform } from '@ionic/angular';
import { Router, NavigationEnd } from '@angular/router';
import { ThemeService } from './shared/services/theme.service';
import { filter } from 'rxjs/operators';
import { NotificationApiService  } from './core/services/notification-api.service';
import { NotificationService } from './core/services/notification.service';
import { Subscription } from 'rxjs';
import { PortalPushMessageService, PortalPushMessageTypes } from './core/services/portal-push-message.service';
import { AuthenticationService } from './authentication/services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {
  showSidenav: boolean = true;
  unreadNotifications: number = 0;
  unreadNotificationsSubscription?: Subscription;
  portalPushMessageSubscription?: Subscription;

  constructor(
    private platform: Platform,
    private zone: NgZone,
    private themeService: ThemeService, //Nicht entfernen!
    private router: Router,
    private notificationService: NotificationService,
    private notificationApiService: NotificationApiService,
    private portalPushMessageService: PortalPushMessageService,
    private authService: AuthenticationService
  ) {
    this.initializeApp();
    this.setupRouterListener();
  }

  async ngOnInit() {
    // Load authentication tokens before starting SignalR connection
    await this.authService.loadTokensFromStorage();

    this.portalPushMessageService.startConnection();

    this.portalPushMessageSubscription = this.portalPushMessageService.portalPushMessageSubject.pipe(
      filter(message => message.type == PortalPushMessageTypes.Notifications)
    )
    .subscribe(() => {
      this.updateNotificationNumber();
    })

    this.unreadNotificationsSubscription = this.notificationService.numberOfUnread.subscribe(numberOfUnread => {
      this.unreadNotifications = numberOfUnread;
    });

    this.updateNotificationNumber();
  }

  ngOnDestroy(): void {
    this.portalPushMessageService.stopConnection();
    this.unreadNotificationsSubscription?.unsubscribe();
    this.portalPushMessageSubscription?.unsubscribe();
  }

  // Nicht entfernen!! Ohne funktioniert Tabellen Styling nicht mehr???
  initializeApp() {
    this.platform.ready().then(() => {
      this.zone.run(() => {
        // Theme service will automatically initialize with saved theme or default to dark
        // No need to force dark mode here anymore
      });
    });
  }

  private updateNotificationNumber(){
    this.notificationApiService.getNotificationsForUser().subscribe((response) => {
      this.notificationService.numberOfUnread.next(response.notifications.filter((n: any) => !n.isRead).length);
    });
  }

  private setupRouterListener() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event) => {
        const navigationEndEvent = event as NavigationEnd;
        this.showSidenav = !navigationEndEvent.url.startsWith('/auth');
      });
  }

  openNotifications() {
    this.router.navigate(['/tabs/notifications']);
  }
}
