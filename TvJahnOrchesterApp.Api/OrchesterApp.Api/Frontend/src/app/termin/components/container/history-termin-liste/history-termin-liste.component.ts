import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, map, switchMap } from 'rxjs';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { TerminListDataResponse } from 'src/app/termin/interfaces/termin-list-data-response';

@Component({
  selector: 'app-history-termin-liste',
  templateUrl: './history-termin-liste.component.html',
  styleUrls: ['./history-termin-liste.component.scss'],
  providers: [Unsubscribe]
})
export class HistoryTerminListeComponent implements OnInit {

  @Input() data!: TerminListDataResponse;
  @Input() canCreateNewTermin!: boolean;

  dataFiltered$!: Observable<TerminListDataResponse>;
  search$ = new BehaviorSubject<string>('');
  currentDate = new Date();

  constructor(
    private router: Router
  ) { }

  ngOnInit(): void {
    this.dataFiltered$ = this.search$.pipe(
      map(searchString => {
        let filteredData = this.data.terminData
          .filter(e => new Date(e.endZeit) < this.currentDate)
          .filter(e => (e.name).toLowerCase().includes(searchString))
          .reverse();
        return {...this.data, terminData: filteredData}
      })
    );
  }

  public search(event: any) {
    let searchString = (event.detail.value as string).toLowerCase();
    this.search$.next(searchString);
  }

  public openTermin(terminId: string) {
    this.router.navigate(['tabs', 'termin', 'details', terminId, 'history']);
  }

}
