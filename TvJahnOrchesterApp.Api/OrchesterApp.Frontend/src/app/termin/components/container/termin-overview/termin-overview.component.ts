import { formatDate } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { TerminListDataResponse } from 'src/app/termin/interfaces/termin-list-data-response';

export enum TerminArt{
  Auftritt = 1,
  Probe,
  Konzert,
  Freizeit,
  Reise
}

export interface HighlightedDates{
    date: string,
    textColor: string,
    backgroundColor: string,
}

@Component({
  selector: 'app-termin-overview',
  templateUrl: './termin-overview.component.html',
  styleUrls: ['./termin-overview.component.scss']
})
export class TerminOverviewComponent implements OnInit {
  private colors = [
    {
      terminArt: TerminArt.Auftritt,
      textColor: 'rgb(68, 10, 184)',
      backgroundColor: 'rgb(211, 200, 229)',
    },
    {
      terminArt: TerminArt.Probe,
      textColor: '#09721b',
      backgroundColor: '#c8e5d0',
    },
    {
      terminArt: TerminArt.Konzert,
      textColor: 'rgb(68, 10, 184)',
      backgroundColor: 'rgb(211, 200, 229)',
    },
    {
      terminArt: TerminArt.Freizeit,
      textColor: 'white',
      backgroundColor: '#8d8d15c2',
    },
    {
      terminArt: TerminArt.Reise,
      textColor: 'rgb(68, 10, 184)',
      backgroundColor: '#8d8d15c2',
    },
  ];

  @Input() data!: TerminListDataResponse;
  public currentDate = new Date();
  public highlightedDates: HighlightedDates[] = [];

  constructor(
    private router: Router
  ) { }

  ngOnInit(): void {
    this.createHighlightedDates();
  }

  public openTermin(terminId: string) {
    this.router.navigate(['tabs', 'termin', 'details', terminId, 'overview']);
  }

  public transformDate(dateString: any){
    return dateString.split('-').reverse().join('.');
  }

  private createHighlightedDates(){
    this.highlightedDates = this.data.terminData.map(t => {
      let selectedColor = this.colors.find(c => c.terminArt === t.terminArt) ?? this.colors[0];
      return {
        date: formatDate(t.startZeit, 'YYYY-MM-dd', 'En-en'),
        textColor: selectedColor.textColor,
        backgroundColor: selectedColor.backgroundColor
      }
    })
  }

}
