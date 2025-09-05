import { Component, NgZone } from '@angular/core';
import { Platform } from '@ionic/angular';
import { Router, NavigationEnd } from '@angular/router';
import { ThemeService } from './shared/services/theme.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  showSidenav: boolean = true;

  constructor(
    private platform: Platform,
    private zone: NgZone,
    private themeService: ThemeService,
    private router: Router
  ) {
    this.initializeApp();
    this.setupRouterListener();
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

  private setupRouterListener() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event) => {
        const navigationEndEvent = event as NavigationEnd;
        this.showSidenav = !navigationEndEvent.url.startsWith('/auth');
      });
  }
}
