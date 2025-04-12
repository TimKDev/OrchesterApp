import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TerminArt } from '../../container/termin-overview/termin-overview.component';

@Component({
  selector: 'app-termin-item',
  templateUrl: './termin-item.component.html',
  styleUrls: ['./termin-item.component.scss'],
})
export class TerminItemComponent  implements OnInit {

  @Input() terminId!: string;
  @Input() terminStatus!: string;
  @Input() terminName!: string;
  @Input() terminArt?: number;
  @Input() terminResponse!: string;
  @Input() istAnwesend!: boolean;
  @Input() terminStartTime!: Date;
  @Input() terminEndTime!: Date;
  @Input() noResponse!: number;
  @Input() positiveResponse!: number;
  @Input() negativeResponse!: number;
  @Input() highAuth!: boolean;
  @Output() openTermin = new EventEmitter<void>();

  terminArtEnum = TerminArt;
  dateNow = new Date();

  constructor() { }

  ngOnInit() {}

  emitOpenTermin(){
    this.openTermin.emit();
  }
}
