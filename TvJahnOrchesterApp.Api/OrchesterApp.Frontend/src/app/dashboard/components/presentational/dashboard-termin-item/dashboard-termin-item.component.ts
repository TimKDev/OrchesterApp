import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TerminArt } from 'src/app/termin/components/container/termin-overview/termin-overview.component';

@Component({
  selector: 'app-dashboard-termin-item',
  templateUrl: './dashboard-termin-item.component.html',
  styleUrls: ['./dashboard-termin-item.component.scss'],
})
export class DashboardTerminItemComponent implements OnInit {

  @Input() terminId!: string;
  @Input() terminName!: string;
  @Input() terminArt?: number;
  @Input() terminResponse!: string;
  @Input() terminStartTime!: Date;
  @Input() terminEndTime!: Date;
  @Output() openTermin = new EventEmitter<void>();

  terminArtEnum = TerminArt;

  constructor() { }

  ngOnInit() { }

  emitOpenTermin() {
    this.openTermin.emit();
  }

}
