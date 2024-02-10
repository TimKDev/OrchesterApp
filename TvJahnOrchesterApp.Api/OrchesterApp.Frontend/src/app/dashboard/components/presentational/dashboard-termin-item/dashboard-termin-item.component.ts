import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-dashboard-termin-item',
  templateUrl: './dashboard-termin-item.component.html',
  styleUrls: ['./dashboard-termin-item.component.scss'],
})
export class DashboardTerminItemComponent implements OnInit {

  @Input() terminId!: string;
  @Input() terminName!: string;
  @Input() terminArt!: string;
  @Input() terminResponse!: string;
  @Input() terminStartTime!: Date;
  @Input() terminEndTime!: Date;
  @Output() openTermin = new EventEmitter<void>();

  constructor() { }

  ngOnInit() { }

  emitOpenTermin() {
    this.openTermin.emit();
  }

}
