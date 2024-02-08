import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { Observable } from 'rxjs';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { GetAllMitgliederResponse } from 'src/app/mitglieder/interfaces/get-all-mitglieder-response';
import { MitgliederService } from 'src/app/mitglieder/services/mitglieder.service';
import { MitgliedCreateModalComponent } from '../mitglied-create-modal/mitglied-create-modal.component';
import { CreateMitgliedRequest } from 'src/app/mitglieder/interfaces/create-mitglied-request';
import { RolesService } from 'src/app/authentication/services/roles.service';

@Component({
  selector: 'app-mitglieder-liste',
  templateUrl: './mitglieder-liste.component.html',
  styleUrls: ['./mitglieder-liste.component.scss'],
  providers: [Unsubscribe]
})
export class MitgliederListeComponent implements OnInit{
  data!: GetAllMitgliederResponse[];
  displayedData!: GetAllMitgliederResponse[];

  canCreateNewMitglied = this.rolesService.isCurrentUserAdmin || this.rolesService.isCurrentUserVorstand;

  @ViewChild('searchBar') searchBar!: any;

  constructor(
    private mitgliederService: MitgliederService,
    private refreshService: RefreshService,
    private us: Unsubscribe,
    private modalCtrl: ModalController,
    private rolesService: RolesService
  ) { }

  ngOnInit(): void {
    if(this.refreshService.needsRefreshing('MitgliederListeComponent')) return;
    this.loadData();
  }

  ionViewWillEnter(){
    if(!this.refreshService.needsRefreshing('MitgliederListeComponent')) return;
    this.loadData();
  }

  loadData(refreshEvent: any = null){
    this.us.autoUnsubscribe(this.mitgliederService.getAllMitglieder()).subscribe(res => {
      this.data = res;
      this.displayedData = res;
      if(refreshEvent) refreshEvent.target.complete();
    });
  }

  search(event: any) {
    let searchString = (event.detail.value as string).toLowerCase();
    this.displayedData = this.data.filter(e => (e.vorname + " " + e.nachname).toLowerCase().includes(searchString));
  }

  public async openCreateOrchesterMitgliedModal(){
    const modal = await this.modalCtrl.create({
      component: MitgliedCreateModalComponent
    });
    modal.present();

    const { data, role } = await modal.onWillDismiss();
    if (role === 'cancel') return;
    this.createMitglied(data);
  }

  public handleRefresh(event: any){
    this.loadData(event);
  }

  private createMitglied(data: CreateMitgliedRequest){
    this.us.autoUnsubscribe(this.mitgliederService.createNewMitglied(data)).subscribe(() => {
      this.loadData();
    })
  }

}
