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
      url: '/tabs/mitglieder/management',
      icon: 'calendar'
    },
    {
      title: 'Hilfe',
      url: '/app/tabs/speakers',
      icon: 'people'
    },
    {
      title: 'Feedback',
      url: '/app/tabs/map',
      icon: 'map'
    },
  ];

  managementPages = [
    {
      title: 'Mitgliedsverwaltung',
      url: '/tabs/mitglieder/management',
      icon: 'calendar'
    },
    {
      title: 'Accountverwaltung',
      url: '/app/tabs/speakers',
      icon: 'people'
    },
    {
      title: 'Terminverwaltung',
      url: '/app/tabs/map',
      icon: 'map'
    }
  ];


  constructor() { }

  ngOnInit() {}

}
