import { Component } from '@angular/core';
import { NEVER, Observable, catchError, tap } from 'rxjs';
import { RolesService } from 'src/app/authentication/services/roles.service';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { TerminListDataResponse } from 'src/app/termin/interfaces/termin-list-data-response';
import { TerminService } from 'src/app/termin/services/termin.service';
import { ModalController } from '@ionic/angular';
import { CreateTerminModalComponent } from '../create-termin-modal/create-termin-modal.component';
import { CreateTerminRequest } from 'src/app/termin/interfaces/create-termin-request';
import { ActivatedRoute, Route } from '@angular/router';

@Component({
  selector: 'app-termin-liste',
  templateUrl: './termin-liste.component.html',
  styleUrls: ['./termin-liste.component.scss'],
  providers: [Unsubscribe]
})
export class TerminListeComponent {

  data$!: Observable<TerminListDataResponse>;
  canCreateNewTermin = this.rolesService.isCurrentUserAdmin || this.rolesService.isCurrentUserVorstand;
  isRefreshing = false;
  defaultSegment = "overview";
  currentlyLoadingWithoutCache = false;

  constructor(
    private terminService: TerminService,
    private refreshService: RefreshService,
    private us: Unsubscribe,
    private rolesService: RolesService,
    private modalCtrl: ModalController,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    let selectedTab = this.route.snapshot.params['activeTab'];
    if(selectedTab !== undefined) this.defaultSegment = selectedTab;
    this.loadData();
  }

  loadData(refreshEvent: any = null, useCache = true) {
    useCache = false; // Arbeite erstmal ohne Cache damit Terminupdates sofort gesehen werden!
    if(this.currentlyLoadingWithoutCache) return;
    this.currentlyLoadingWithoutCache = !useCache;
    this.data$ = this.terminService.getAllTermins(useCache).pipe(
      tap((data) => {
        data.terminData.forEach(t => t.startZeit = new Date(t.startZeit));
        data.terminData.forEach(t => t.endZeit = new Date(t.endZeit));
        if (refreshEvent){
          refreshEvent.target.complete();
          this.isRefreshing = false;
        }
        this.currentlyLoadingWithoutCache = false;
      }),
      catchError(() => {this.currentlyLoadingWithoutCache = false; return NEVER})
    );
  }

  ionViewWillEnter() {
    if (!this.refreshService.needsRefreshing('TerminListeComponent')) return;
    this.loadData(null, false);
  }

  public handleRefresh(event: any) {
    this.isRefreshing = true;
    this.loadData(event, false);
  }

  public async openCreateTerminModal(){
    const modal = await this.modalCtrl.create({
      component: CreateTerminModalComponent
    });
    modal.present();

    const { data, role } = await modal.onWillDismiss();
    if (role === 'cancel') return;
    this.createTermin(data);
  }

  private createTermin(data: CreateTerminRequest){
    this.us.autoUnsubscribe(this.terminService.createNewTermin(data)).subscribe(() => {
      this.loadData(null, false);
    })
  }
}
