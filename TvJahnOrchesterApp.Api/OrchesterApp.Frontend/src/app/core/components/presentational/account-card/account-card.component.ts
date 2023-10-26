import { Component, EventEmitter, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-account-card',
  templateUrl: './account-card.component.html',
  styleUrls: ['./account-card.component.scss'],
})
export class AccountCardComponent implements OnInit {

  @Input() name!: string;
  @Input() email: string | null = null;
  @Input() lastLogin: Date | null = null;
  @Input() color!: string;

  constructor() { }

  ngOnInit() {}
}
