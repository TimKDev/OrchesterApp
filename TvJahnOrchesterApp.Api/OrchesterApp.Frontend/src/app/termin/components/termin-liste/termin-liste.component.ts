import { Component, OnInit } from '@angular/core';
import { TerminService } from '../../services/termin.service';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { GetAllTermineResponse } from '../../interfaces/get-all-termine-response';
import { ModalController } from '@ionic/angular';
import { RolesService } from 'src/app/authentication/services/roles.service';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { CreateTerminModalComponent } from '../create-termin-modal/create-termin-modal.component';
import { CreateTerminRequest } from '../../interfaces/create-termin-request';

@Component({
  selector: 'app-termin-liste',
  templateUrl: './termin-liste.component.html',
  styleUrls: ['./termin-liste.component.scss'],
  providers: [Unsubscribe]
})
export class TerminListeComponent  implements OnInit {

  data!: GetAllTermineResponse[];
  displayedData!: GetAllTermineResponse[];

  canCreateNewTermin = this.rolesService.isCurrentUserAdmin || this.rolesService.isCurrentUserVorstand;

  constructor(
    private terminService: TerminService,
    private refreshService: RefreshService,
    private us: Unsubscribe,
    private modalCtrl: ModalController,
    private rolesService: RolesService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  ionViewWillEnter(){
    if(!this.refreshService.needsRefreshing('TerminListeComponent')) return;
    this.loadData();
  }

  loadData(refreshEvent: any = null){
    this.us.autoUnsubscribe(this.terminService.getAllTermins()).subscribe(res => {
      this.data = res;
      this.displayedData = res;
      if(refreshEvent) refreshEvent.target.complete();
    });
  }

  public handleRefresh(event: any){
    this.loadData(event);
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
      this.loadData();
    })
  }

}
