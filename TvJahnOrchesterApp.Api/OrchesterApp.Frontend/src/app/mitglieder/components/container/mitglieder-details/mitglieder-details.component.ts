import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalController } from '@ionic/angular';
import { Observable, combineLatest, map, tap } from 'rxjs';
import { confirmDialog } from 'src/app/core/helper/confirm';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { DropdownService } from 'src/app/core/services/dropdown.service';
import { GetSpecificMitgliederResponse } from 'src/app/mitglieder/interfaces/get-specific-mitglieder-response';
import { MitgliederService } from 'src/app/mitglieder/services/mitglieder.service';
import { MitgliedAdminUpdateModalComponent } from '../mitglied-admin-update-modal/mitglied-admin-update-modal.component';
import { UpdateSpecificMitgliederRequest } from 'src/app/mitglieder/interfaces/update-specific-mitglieder-request';
import { UpdateAdminSpecificMitgliederRequest } from 'src/app/mitglieder/interfaces/update-admin-specific-mitglieder-request';
import { RefreshService } from 'src/app/core/services/refresh.service';
import { MitgliedUpdateModalComponent } from '../mitglied-update-modal/mitglied-update-modal.component';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';

@Component({
  selector: 'app-mitglieder-details',
  templateUrl: './mitglieder-details.component.html',
  styleUrls: ['./mitglieder-details.component.scss'],
  providers: [Unsubscribe]
})
export class MitgliederDetailsComponent implements OnInit {

  data$!: Observable<{ data: GetSpecificMitgliederResponse, instrumentDropdown: DropdownItem[], notenStimmeDropdown: DropdownItem[], positionDropdown: DropdownItem[], mitgliedsStatusDropdown: DropdownItem[]}> | null;
  mitgliedsId!: string;
  dropdownItemsInstruments!: DropdownItem[];
  dropdownItemsNotenstimme!: DropdownItem[];
  dropdownItemsPosition!: DropdownItem[];
  dropdownItemsMitgliedsStatus!: DropdownItem[];

  constructor(
    private mitgliederService: MitgliederService,
    private route: ActivatedRoute,
    private dropdownService: DropdownService,
    private router: Router,
    private us: Unsubscribe,
    private modalCtrl: ModalController,
    private refreshService: RefreshService,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit() {
    this.mitgliedsId = this.route.snapshot.params["mitgliedsId"];
    this.loadData();
  }

  isActionSheetOpen = false;

  setOpen(isOpen: boolean) {
    this.isActionSheetOpen = isOpen;
  }

  getLoggedInOrchesterMitgliedsName(){
    return this.authenticationService.connectedOrchesterMitgliedsName;
  }

  loadData() {
    this.data$ = this.us.autoUnsubscribe(combineLatest([
      this.mitgliederService.getSpecificMitglied(this.mitgliedsId),
      this.dropdownService.getDropdownElements('Instrument'),
      this.dropdownService.getDropdownElements('Notenstimme'),
      this.dropdownService.getDropdownElements('Position'),
      this.dropdownService.getDropdownElements('MitgliedsStatus'),
    ])).pipe(
      map(([data, instrumentDropdown, notenStimmeDropdown, positionDropdown, mitgliedsStatusDropdown]) => ({ data, instrumentDropdown, notenStimmeDropdown, positionDropdown, mitgliedsStatusDropdown })),
      tap(result => {
        this.dropdownItemsInstruments = result.instrumentDropdown;
        this.dropdownItemsNotenstimme = result.notenStimmeDropdown;
        this.dropdownItemsPosition = result.positionDropdown;
        this.dropdownItemsMitgliedsStatus = result.mitgliedsStatusDropdown;
      })
    );
  }

  public actionSheetButtons = [
    {
      text: 'Lösche Orchestermitglied',
      role: 'destructive',
      handler: () => this.deleteOrchesterMember()

    },
    {
      text: 'Bearbeite Orchestermitglied',
      data: {
        action: 'share',
      },
      handler: () => this.openAdminUpdateOrchesterMemberModal()
    },
    {
      text: 'Zurück',
      role: 'cancel',
      data: {
        action: 'cancel',
      },
    },
  ];

  @confirmDialog("Achtung", "Möchten sie dieses Mitglied wirklich löschen? Falls ein Account verbunden ist, wird dieser ebenfalls gelöscht! Diese Operation kann nicht rückgängig gemacht werden.")
  private deleteOrchesterMember() {
    if (!this.mitgliedsId) return;
    this.data$ = null;
    this.mitgliederService.deleteMitglied(this.mitgliedsId).subscribe(() => {
      this.router.navigate(['tabs', 'mitglieder']);
    })
  }

  private async openAdminUpdateOrchesterMemberModal() {
    const modal = await this.modalCtrl.create({
      component: MitgliedAdminUpdateModalComponent,
      componentProps: {
        "mitgliedsId": this.mitgliedsId,
        "dropdownItemsNotenstimme": this.dropdownItemsNotenstimme,
        "dropdownItemsInstruments": this.dropdownItemsInstruments,
        "dropdownItemsPosition": this.dropdownItemsPosition,
        "dropdownItemsMitgliedsStatus": this.dropdownItemsMitgliedsStatus,
      }
    });
    modal.present();
    this.setOpen(false);

    const { data, role } = await modal.onWillDismiss();
    if (role === 'cancel') return;
    this.updateAdminData(data);
  }

  private updateAdminData(data: any){
    this.refreshService.refreshComponent("MitgliederListeComponent");
    this.us.autoUnsubscribe(this.mitgliederService.updateAdminSpecificMitglied({...data, id: this.mitgliedsId} as UpdateAdminSpecificMitgliederRequest)).subscribe(() => {
      this.loadData();
    })
  }

  public async openUpdateOrchesterMitgliedModal(){
    const modal = await this.modalCtrl.create({
      component: MitgliedUpdateModalComponent,
      componentProps: {
        "mitgliedsId": this.mitgliedsId,
      }
    });
    modal.present();

    const { data, role } = await modal.onWillDismiss();
    if (role === 'cancel') return;
    this.updateData(data);
  }

  private updateData(data: any){
    this.refreshService.refreshComponent("MitgliederListeComponent");
    this.us.autoUnsubscribe(this.mitgliederService.updateSpecificMitglied({...data, id: this.mitgliedsId} as UpdateSpecificMitgliederRequest)).subscribe(() => {
      this.loadData();
    })
  }

}
