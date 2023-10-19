import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss'],
})
export class SideMenuComponent implements OnInit {

  mainPages = [
    {
      title: 'Einstellungen',
      url: '/tabs/settings',
      icon: 'calendar'
    },
    {
      title: 'Hilfe',
      url: '/tabs/help',
      icon: 'people'
    },
    {
      title: 'Feedback',
      url: '/tabs/feedback',
      icon: 'map'
    },
  ];

  managementPages = [
    {
      title: 'Accountverwaltung',
      url: '/tabs/account-management',
      icon: 'people'
    }
  ];


  constructor(public authService: AuthenticationService, private router: Router) { }

  ngOnInit() { }

  async logout() {
    await this.authService.logout();
    this.router.navigateByUrl('auth', { replaceUrl: true });
  }

}
