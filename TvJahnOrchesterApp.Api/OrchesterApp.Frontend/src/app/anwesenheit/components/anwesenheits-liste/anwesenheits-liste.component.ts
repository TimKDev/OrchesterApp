import { Component, OnInit } from '@angular/core';
import { AnwesenheitService } from '../../services/anwesenheit.service';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { AnwesenheitsListeGetResponseEntry } from '../../interfaces/anwesenheits-liste-get-response';
import { Observable, tap } from 'rxjs';
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

  private YEAR = 2024;

  constructor(
    private anwesenheitsService: AnwesenheitService,
    private refreshService: RefreshService,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.nameOfCurrentUser = this.authenticationService.connectedOrchesterMitgliedsName!;
    if (this.refreshService.needsRefreshing('TerminListeComponent')) return;
    this.loadData();
  }

  loadData(refreshEvent: any = null) {
    this.data$ = this.anwesenheitsService.getAnwesenheitsListeForYear(this.YEAR).pipe(
      tap(() => {
        if (refreshEvent) {
          refreshEvent.target.complete();
          this.isRefreshing = false;
        }
      })
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
