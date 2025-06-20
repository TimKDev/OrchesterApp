import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ModalController } from '@ionic/angular';
import { Observable, tap } from 'rxjs';
import { TerminResponseResponse, TerminRückmeldungsTableEntry } from 'src/app/termin/interfaces/termin-response-response';
import { TerminService } from 'src/app/termin/services/termin.service';
import { TerminReturnMessageDetailsComponent } from '../termin-return-message-details/termin-return-message-details.component';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { UpdateTerminResponseRequest } from 'src/app/termin/interfaces/update-termin-response-request';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { TerminAnwesenheitControlModalComponent } from '../termin-anwesenheit-control-modal/termin-anwesenheit-control-modal.component';
import { UpdateTerminAnwesenheitsRequest } from 'src/app/termin/interfaces/update-termin-anwesenheits-request';

@Component({
  selector: 'app-termin-return-messages',
  templateUrl: './termin-return-messages.component.html',
  styleUrls: ['./termin-return-messages.component.scss'],
})
export class TerminReturnMessagesComponent implements OnInit {

  public terminId!: string;
  public isRefreshing = false;
  public data$!: Observable<TerminResponseResponse>;
  public currentDateTime = new Date();

  constructor(
    private terminService: TerminService,
    private route: ActivatedRoute,
    private modalCtrl: ModalController,
    private refreshService: RefreshService,
  ) { }

  ngOnInit() {
    this.terminId = this.route.snapshot.params['terminId'];
    this.loadData();
  }

  public handleRefresh(event: any) {
    this.isRefreshing = true;
    this.loadData(event);
  }

  public async openResponseDetails(response: TerminRückmeldungsTableEntry, responseDropdownValues: DropdownItem[], showAnwesenheit: boolean) {
    const modal = await this.modalCtrl.create({
      component: TerminReturnMessageDetailsComponent,
      componentProps: {
        "response": response,
        "responseDropdownValues": responseDropdownValues,
        "showAnwesenheit": showAnwesenheit
      }
    });
    modal.present();
    const { data, role } = await modal.onWillDismiss();
    if (role === 'cancel') return;
    this.updateResponseDetails(data);
  }

  public async openAnwesenheitsControlling(responses: TerminRückmeldungsTableEntry[]) {
    const modal = await this.modalCtrl.create({
      component: TerminAnwesenheitControlModalComponent,
      componentProps: {
        "dataResponses": responses
      }
    });
    modal.present();
    const { data, role } = await modal.onWillDismiss();
    if (role === 'cancel') return;
    this.updateAnwesenheitsResponses(data);
  }

  private updateAnwesenheitsResponses(data: any){
    let dataPut: UpdateTerminAnwesenheitsRequest = { anwesenheitResponseEntries: data.items, terminId: this.terminId };
    this.terminService.updateTerminAnwesenheitControl(dataPut).subscribe(() => {
      this.loadData(null);
      this.refreshService.refreshComponent('TerminListeComponent');
      this.refreshService.refreshComponent('TerminDetails');
      this.refreshService.refreshComponent('Dashboard');
    });
  }

  private updateResponseDetails(data: UpdateTerminResponseRequest) {
    data = { ...data, terminId: this.terminId };
    this.terminService.updateTerminResponseDetails(data).subscribe(() => {
      this.loadData(null);
      this.refreshService.refreshComponent('TerminListeComponent');
      this.refreshService.refreshComponent('TerminDetails');
      this.refreshService.refreshComponent('Dashboard');
    });
  }

  private loadData(refreshEvent: any = null) {
    this.data$ = this.terminService.getTerminResponses(this.terminId).pipe(
      tap((data) => {
        data.terminRückmeldungsTableEntries.forEach(entry => {
          entry.letzteRückmeldung = entry.letzteRückmeldung ? new Date(entry.letzteRückmeldung) : undefined
        });
        data.startZeit = new Date(data.startZeit);
        data.startZeit.setHours(0, 0, 0);
        if (refreshEvent) {
          refreshEvent.target.complete();
          this.isRefreshing = false;
        }
      })
    );
  }

}
