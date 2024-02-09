import { Component, OnInit } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { DashboardService } from '../../services/dashboard.service';
import { DashboardGetResponse } from '../../interfaces/dashboard-get-response';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit{
  
  data$!: Observable<DashboardGetResponse>;
  isRefreshing = false;

  constructor(
    private dashboardService: DashboardService,
    private refreshService: RefreshService
  ) { }

  ngOnInit(): void {
    if (this.refreshService.needsRefreshing('Dashboard')) return;
    this.loadData();
  }

  loadData(refreshEvent: any = null) {
    this.data$ = this.dashboardService.getDashboardData().pipe(
      tap(() => {
        if (refreshEvent) {
          refreshEvent.target.complete();
          this.isRefreshing = false;
        }
      })
    );
  }

  ionViewWillEnter() {
    if (!this.refreshService.needsRefreshing('Dashboard')) return;
    this.loadData(null);
  }

  public handleRefresh(event: any) {
    this.isRefreshing = true;
    this.loadData(event);
  }
}
