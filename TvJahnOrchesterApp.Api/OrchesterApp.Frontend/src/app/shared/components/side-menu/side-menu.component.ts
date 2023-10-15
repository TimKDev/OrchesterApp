import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss'],
})
export class SideMenuComponent  implements OnInit {

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


  constructor() { }

  ngOnInit() {}

}
