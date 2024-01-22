import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ModalController } from '@ionic/angular';
import { Observable, tap } from 'rxjs';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { TerminDetailsResponse } from 'src/app/termin/interfaces/termin-details-response';
import { TerminService } from 'src/app/termin/services/termin.service';
import { UpdateTerminModalComponent } from '../update-termin-modal/update-termin-modal.component';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';

@Component({
  selector: 'app-termin-details',
  templateUrl: './termin-details.component.html',
  styleUrls: ['./termin-details.component.scss'],
  providers: [Unsubscribe]
})
export class TerminDetailsComponent  implements OnInit {

  activeTab!: string;
  terminId!: string;
  data$!: Observable<TerminDetailsResponse>;
  isRefreshing = false;
  dateNow = new Date();

  constructor(
    private route: ActivatedRoute,
    private terminService: TerminService,
    private refreshService: RefreshService,
    private modalCtrl: ModalController,
    private us: Unsubscribe,
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

  public openResponseAlert(){

  }

  public async openUpdateModal(){
    const modal = await this.modalCtrl.create({
      component: UpdateTerminModalComponent
    });
    modal.present();

    const { data, role } = await modal.onWillDismiss();
    if (role === 'cancel') return;
    this.updateTermin(data);
  }

  private updateTermin(data: any){
    // this.us.autoUnsubscribe(this.terminService.createNewTermin(data)).subscribe(() => {
    //   this.loadData(null, false);
    // })
  }

}
