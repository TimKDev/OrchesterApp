import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { TerminListDataResponse } from 'src/app/termin/interfaces/termin-list-data-response';

@Component({
  selector: 'app-termin-overview',
  templateUrl: './termin-overview.component.html',
  styleUrls: ['./termin-overview.component.scss']
})
export class TerminOverviewComponent implements OnInit {
  highlightedDates = [
    {
      date: '2023-12-05',
      textColor: '#09721b',
      backgroundColor: '#c8e5d0',
    },
    {
      date: '2023-12-10',
      textColor: '#09721b',
      backgroundColor: '#c8e5d0',
    },
    {
      date: '2023-12-20',
      textColor: '#09721b',
      backgroundColor: '#c8e5d0',
    },
    {
      date: '2023-12-23',
      textColor: 'rgb(68, 10, 184)',
      backgroundColor: 'rgb(211, 200, 229)',
    },
  ];

  @Input() data!: TerminListDataResponse;
  currentDate = new Date();

  constructor(
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  public openTermin(terminId: string) {
    this.router.navigate(['tabs', 'termin', 'details', terminId]);
  }

}
