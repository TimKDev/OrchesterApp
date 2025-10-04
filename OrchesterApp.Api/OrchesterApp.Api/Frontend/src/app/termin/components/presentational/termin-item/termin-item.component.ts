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
  @Input() image?: string;
  @Input() fristAsDate?: Date;
  @Input() ersteWarnungVorFristAsDate?: Date;
  @Output() openTermin = new EventEmitter<void>();

  terminArtEnum = TerminArt;
  dateNow = new Date();

  constructor() { }

  ngOnInit() {}

  emitOpenTermin(){
    this.openTermin.emit();
  }

  getResponseColorClass(): string {
    if (this.terminResponse !== 'Nicht Zurückgemeldet') {
      return this.terminResponse === 'Zugesagt' ? 'green' : '';
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
