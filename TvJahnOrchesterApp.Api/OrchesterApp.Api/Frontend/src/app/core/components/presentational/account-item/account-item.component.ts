import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-account-item',
  templateUrl: './account-item.component.html',
  styleUrls: ['./account-item.component.scss'],
})
export class AccountItemComponent  implements OnInit {

  @Input() name!: string;
  @Input() email: string | null = null;
  @Input() lastLogin: Date | null = null;
  @Input() connected!: boolean;

  constructor() { }

  ngOnInit() {}

}
