import { Component, OnInit } from '@angular/core';
import { NEVER, Observable, catchError, tap } from 'rxjs';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { DashboardService } from '../../../services/dashboard.service';
import { DashboardGetResponse } from '../../../interfaces/dashboard-get-response';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit{
  
  data$!: Observable<DashboardGetResponse>;
  isRefreshing = false;
  currentlyLoading = false;

  constructor(
    private dashboardService: DashboardService,
    private refreshService: RefreshService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(refreshEvent: any = null) {
    if(this.currentlyLoading) return;
    this.currentlyLoading = true;
    this.data$ = this.dashboardService.getDashboardData().pipe(
      tap((data) => {
        data.birthdayList.sort((a, b) => {
          let birthA = new Date(a.birthday);
          let birthB = new Date(b.birthday);
          birthA.setFullYear((new Date()).getFullYear());
          birthB.setFullYear((new Date()).getFullYear());
          return birthA.getTime() - birthB.getTime()
        })
        if (refreshEvent) {
          refreshEvent.target.complete();
          this.isRefreshing = false;
        }
        this.currentlyLoading = false;
      }),
      catchError(() => {this.currentlyLoading = false; return NEVER})
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

  openTermin(terminId: string){
    this.router.navigate(['tabs', 'termin', 'details', terminId, 'default']);
  }
}
