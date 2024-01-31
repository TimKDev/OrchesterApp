import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { TerminResponseResponse } from 'src/app/termin/interfaces/termin-response-response';
import { TerminService } from 'src/app/termin/services/termin.service';

@Component({
  selector: 'app-termin-return-messages',
  templateUrl: './termin-return-messages.component.html',
  styleUrls: ['./termin-return-messages.component.scss'],
})
export class TerminReturnMessagesComponent implements OnInit {

  public terminId!: string;
  public isRefreshing = false;
  public data$!: Observable<TerminResponseResponse>;

  constructor(
    private terminService: TerminService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.terminId = this.route.snapshot.params['terminId'];
    this.loadData();
  }

  public handleRefresh(event: any) {
    this.isRefreshing = true;
    this.loadData(event);
  }

  private loadData(refreshEvent: any = null) {
    this.data$ = this.terminService.getTerminResponses(this.terminId).pipe(
      tap((data) => {
        data.terminRückmeldungsTableEntries.forEach(entry => {
          entry.letzteRückmeldung = new Date(entry.letzteRückmeldung + 'Z')
        });
        if (refreshEvent) {
          refreshEvent.target.complete();
          this.isRefreshing = false;
        }
      })
    );
  }

}
