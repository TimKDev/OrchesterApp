import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, pipe, tap } from 'rxjs';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { TerminDetailsResponse } from 'src/app/termin/interfaces/termin-details-response';
import { TerminService } from 'src/app/termin/services/termin.service';

@Component({
  selector: 'app-termin-details',
  templateUrl: './termin-details.component.html',
  styleUrls: ['./termin-details.component.scss'],
})
export class TerminDetailsComponent  implements OnInit {

  activeTab!: string;
  terminId!: string;
  data$!: Observable<TerminDetailsResponse>;
  isRefreshing = false;

  constructor(
    private route: ActivatedRoute,
    private terminService: TerminService,
    private refreshService: RefreshService
  ) { }

  ngOnInit() {
    this.activeTab = this.route.snapshot.params['activeTab'];
    this.terminId = this.route.snapshot.params['terminId'];
    this.loadData();
  }

  loadData(refreshEvent: any = null) {
    this.data$ = this.terminService.getTerminDetails(this.terminId).pipe(
      tap(() => {
        if(refreshEvent){
          refreshEvent.target.complete();
          this.isRefreshing = false;
        } 
      })
    );
  }

  ionViewWillEnter() {
    if (!this.refreshService.needsRefreshing('TerminListeComponent')) return;
    this.loadData();
  }

  public handleRefresh(event: any) {
    this.isRefreshing = true;
    this.loadData(event);
  }

}
