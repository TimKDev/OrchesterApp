import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, map, switchMap } from 'rxjs';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { TerminListDataResponse } from 'src/app/termin/interfaces/termin-list-data-response';

@Component({
  selector: 'app-current-termin-liste',
  templateUrl: './current-termin-liste.component.html',
  styleUrls: ['./current-termin-liste.component.scss'],
  providers: [Unsubscribe]
})
export class CurrentTerminListeComponent  implements OnInit {
  @Input() data!: TerminListDataResponse;
  @Input() canCreateNewTermin!: boolean;
  @Output() createTermin = new EventEmitter<void>();

  dataFiltered$!: Observable<TerminListDataResponse>;
  search$ = new BehaviorSubject<string>('');
  currentDate = new Date();

  constructor(
    private router: Router
  ) { }

  ngOnInit(): void {
    this.dataFiltered$ = this.search$.pipe(
      map(searchString => {
        let filteredData = this.data.terminData.filter(e => new Date(e.endZeit) >= this.currentDate).filter(e => (e.name).toLowerCase().includes(searchString));
        return {...this.data, terminData: filteredData}
      })
    );
  }

  public search(event: any) {
    let searchString = (event.detail.value as string).toLowerCase();
    this.search$.next(searchString);
  }

  public openTermin(terminId: string) {
    this.router.navigate(['tabs', 'termin', 'details', terminId, 'default']);
  }

  public openCreateTerminModal(){
    this.createTermin.emit();
  }

}
