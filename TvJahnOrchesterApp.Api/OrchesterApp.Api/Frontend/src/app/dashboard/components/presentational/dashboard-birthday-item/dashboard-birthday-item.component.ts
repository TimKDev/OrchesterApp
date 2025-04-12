import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard-birthday-item',
  templateUrl: './dashboard-birthday-item.component.html',
  styleUrls: ['./dashboard-birthday-item.component.scss'],
})
export class DashboardBirthdayItemComponent  implements OnInit {

  @Input() name!: string;
  @Input() image?: string;
  @Input() birthday!: Date;

  constructor() { }

  ngOnInit() {}

}
