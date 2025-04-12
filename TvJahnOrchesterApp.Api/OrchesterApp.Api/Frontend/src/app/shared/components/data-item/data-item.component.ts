import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-data-item',
  templateUrl: './data-item.component.html',
  styleUrls: ['./data-item.component.scss'],
})
export class DataItemComponent  implements OnInit {
  @Input() label!: string;
  @Input() value!: string | null;
  @Input() unknownValue: string = "unbekannt";

  constructor() { }

  ngOnInit() {}

}
