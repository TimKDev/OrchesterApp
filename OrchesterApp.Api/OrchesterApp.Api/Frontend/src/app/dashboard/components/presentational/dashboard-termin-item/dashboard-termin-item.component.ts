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
  @Input() image?: string;
  @Input() fristAsDate?: Date;
  @Input() ersteWarnungVorFristAsDate?: Date;
  @Output() openTermin = new EventEmitter<void>();

  terminArtEnum = TerminArt;

  constructor() { }

  ngOnInit() { }

  emitOpenTermin() {
    this.openTermin.emit();
  }

  getResponseColorClass(): string {
    if (this.terminResponse !== 'Nicht Zurückgemeldet') {
      return '';
    }

    const now = new Date();
    const oneWeekFromNow = new Date(now.getTime() + 7 * 24 * 60 * 60 * 1000);
    const terminStart = new Date(this.terminStartTime);
    
    // Red if deadline passed OR termin starts in less than one week
    if ((this.fristAsDate && new Date(this.fristAsDate) < now) || terminStart < oneWeekFromNow) {
      return 'red';
    }
    if (this.ersteWarnungVorFristAsDate && new Date(this.ersteWarnungVorFristAsDate) < now) {
      return 'yellow';
    }
    return '';
  }

  getResponseText(): string {
    if (this.terminResponse !== 'Nicht Zurückgemeldet') {
      return this.terminResponse;
    }

    const now = new Date();
    if (this.ersteWarnungVorFristAsDate && new Date(this.ersteWarnungVorFristAsDate) < now && 
        (!this.fristAsDate || new Date(this.fristAsDate) >= now)) {
      return 'Frist läuft bald ab';
    }
    return this.terminResponse;
  }

}
