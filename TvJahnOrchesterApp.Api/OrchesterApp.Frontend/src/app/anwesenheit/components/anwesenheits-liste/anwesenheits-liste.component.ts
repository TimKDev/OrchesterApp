import { Component, OnInit } from '@angular/core';
import { AnwesenheitService } from '../../services/anwesenheit.service';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { AnwesenheitsListeGetResponseEntry } from '../../interfaces/anwesenheits-liste-get-response';
import { NEVER, Observable, catchError, tap } from 'rxjs';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';

@Component({
  selector: 'app-anwesenheits-liste',
  templateUrl: './anwesenheits-liste.component.html',
  styleUrls: ['./anwesenheits-liste.component.scss']
})
export class AnwesenheitsListeComponent implements OnInit {
  data$!: Observable<AnwesenheitsListeGetResponseEntry[]>;
  isRefreshing = false;
  defaultSegment = "default";
  nameOfCurrentUser = "";
  currentlyLoading = false;

  private YEAR = 2024;

  constructor(
    private anwesenheitsService: AnwesenheitService,
    private refreshService: RefreshService,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.nameOfCurrentUser = this.authenticationService.connectedOrchesterMitgliedsName!;
    this.loadData();
  }

  loadData(refreshEvent: any = null) {
    if(this.currentlyLoading) return;
    this.currentlyLoading = true;
    this.data$ = this.anwesenheitsService.getAnwesenheitsListeForYear(this.YEAR).pipe(
      tap(() => {
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
    if (!this.refreshService.needsRefreshing('Anwesenheitsliste')) return;
    this.loadData(null);
  }

  public handleRefresh(event: any) {
    this.isRefreshing = true;
    this.loadData(event);
  }

}
