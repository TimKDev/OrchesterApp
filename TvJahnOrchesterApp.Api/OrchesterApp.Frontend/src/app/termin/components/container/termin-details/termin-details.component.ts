import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalController } from '@ionic/angular';
import { Observable, map, tap } from 'rxjs';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { TerminDetailsResponse } from 'src/app/termin/interfaces/termin-details-response';
import { TerminService } from 'src/app/termin/services/termin.service';
import { UpdateTerminModalComponent } from '../update-termin-modal/update-termin-modal.component';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { TerminResponseModalComponent } from '../termin-response-modal/termin-response-modal.component';
import { UpdateTerminResponseRequest } from 'src/app/termin/interfaces/update-termin-response-request';
import { UpdateTerminRequest } from 'src/app/termin/interfaces/update-termin-request';
import { RolesService } from 'src/app/authentication/services/roles.service';

@Component({
  selector: 'app-termin-details',
  templateUrl: './termin-details.component.html',
  styleUrls: ['./termin-details.component.scss'],
  providers: [Unsubscribe]
})
export class TerminDetailsComponent implements OnInit {

  activeTab!: string;
  canEditTermin = this.rolesService.isCurrentUserAdmin || this.rolesService.isCurrentUserVorstand;
  terminId!: string;
  data$!: Observable<TerminDetailsResponse>;
  isRefreshing = false;
  dateNow = new Date();
  noten = '';
  uniform = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private terminService: TerminService,
    private refreshService: RefreshService,
    private modalCtrl: ModalController,
    private us: Unsubscribe,
    private rolesService: RolesService
  ) { }

  ngOnInit() {
    this.activeTab = this.route.snapshot.params['activeTab'];
    this.terminId = this.route.snapshot.params['terminId'];
    this.loadData();
  }

  loadData(refreshEvent: any = null) {
    this.data$ = this.terminService.getTerminDetails(this.terminId).pipe(
      tap((data) => {
        data.termin.startZeit = new Date(data.termin.startZeit + 'Z');
        data.termin.endZeit = new Date(data.termin.endZeit + 'Z');
        if (refreshEvent) {
          refreshEvent.target.complete();
          this.isRefreshing = false;
        }
      })
    );
  }

  ionViewWillEnter() {
    if (!this.refreshService.needsRefreshing('TerminDetails')) return;
    this.loadData();
  }

  public handleRefresh(event: any) {
    this.isRefreshing = true;
    this.loadData(event);
  }

  isActionSheetOpen = false;

  setOpen(isOpen: boolean) {
    this.isActionSheetOpen = isOpen;
  }

  public actionSheetButtons = [
    {
      text: 'Lösche Termin',
      role: 'destructive',
      handler: () => this.deleteTermin()

    },
    {
      text: 'Bearbeite Anwesenheit',
      data: {
        action: 'share',
      },
      handler: () => this.navigateToTerminReturnMessages()
    },
    {
      text: 'Zurück',
      role: 'cancel',
      data: {
        action: 'cancel',
      },
    },
  ];

  public deleteTermin(){

  }

  public navigateToTerminReturnMessages(){
    this.router.navigate(['tabs', 'termin', 'return-messages', this.terminId]);
  }

  public async openResponseModal(dataTermin: TerminDetailsResponse) {
    const modal = await this.modalCtrl.create({
      component: TerminResponseModalComponent,
      componentProps: {
        "dataTermin": dataTermin
      }
    });
    modal.present();
    const { data, role } = await modal.onWillDismiss();
    if (role === 'cancel') return;
    this.updateResponse(data);
  }

  public async openUpdateModal(dataTermin: TerminDetailsResponse) {
    const modal = await this.modalCtrl.create({
      component: UpdateTerminModalComponent,
      componentProps: {
        "dataTermin": dataTermin
      }
    });
    modal.present();
    const { data, role } = await modal.onWillDismiss();
    if (role === 'cancel') return;
    data.terminId = this.terminId;
    this.updateTermin(data);
  }

  private updateTermin(data: UpdateTerminRequest) {
    this.us.autoUnsubscribe(this.terminService.updateTerminDetails(data)).subscribe(() => {
      this.loadData(null);
      this.refreshService.refreshComponent('TerminListeComponent');
    });
  }

  private updateResponse(data: UpdateTerminResponseRequest) {
    let dataWithTermin = {...data, terminId: this.terminId};
    this.us.autoUnsubscribe(this.terminService.updateTerminResponse(dataWithTermin)).subscribe(() => {
      this.loadData(null);
      this.refreshService.refreshComponent('TerminListeComponent');
    });
  }

}
