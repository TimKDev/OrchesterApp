import { Component, NgZone } from '@angular/core';
import { Platform } from '@ionic/angular';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  constructor(private platform: Platform, private zone: NgZone) {
    this.initializeApp();
  }

  initializeApp() {
    this.platform.ready().then(() => {
      this.zone.run(() => {
        this.forceDarkMode();
      });
    });
  }

  forceDarkMode() {
    document.body.classList.add('dark-theme'); // Add your custom class for dark mode
  }
}
