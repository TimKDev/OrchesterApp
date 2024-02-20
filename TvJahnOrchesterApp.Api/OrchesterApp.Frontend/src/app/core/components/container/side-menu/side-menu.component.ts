import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';
import { RolesService } from 'src/app/authentication/services/roles.service';

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
      icon: 'settings'
    },
    {
      title: 'Hilfe',
      url: '/tabs/help',
      icon: 'help-circle'
    },
    {
      title: 'Impressum und Datenschutz',
      url: '/tabs/impressum',
      icon: 'document-text'
    },
  ];

  managementPages = [
    {
      title: 'Accountverwaltung',
      url: '/tabs/account-management',
      icon: 'people'
    }
  ];

  constructor(
    public authService: AuthenticationService, 
    public rolesService: RolesService,
    private router: Router) { }

  ngOnInit() { }

  async logout() {
    await this.authService.logout();
    this.router.navigateByUrl('auth', { replaceUrl: true });
  }

}
