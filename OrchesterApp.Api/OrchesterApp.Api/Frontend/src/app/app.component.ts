import { Component, NgZone } from '@angular/core';
import { Platform } from '@ionic/angular';
import { ThemeService } from './shared/services/theme.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  constructor(
    private platform: Platform, 
    private zone: NgZone,
    private themeService: ThemeService
  ) {
    this.initializeApp();
  }

  initializeApp() {
    this.platform.ready().then(() => {
      this.zone.run(() => {
        // Theme service will automatically initialize with saved theme or default to dark
        // No need to force dark mode here anymore
      });
    });
  }
}
