import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalController } from '@ionic/angular';
import { Observable, combineLatest, map } from 'rxjs';
import { confirmDialog } from 'src/app/core/helper/confirm';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { DropdownService } from 'src/app/core/services/dropdown.service';
import { GetSpecificMitgliederResponse } from 'src/app/mitglieder/interfaces/get-specific-mitglieder-response';
import { MitgliederService } from 'src/app/mitglieder/services/mitglieder.service';
import { MitgliedAdminUpdateModalComponent } from '../mitglied-admin-update-modal/mitglied-admin-update-modal.component';

@Component({
  selector: 'app-mitglieder-details',
  templateUrl: './mitglieder-details.component.html',
  styleUrls: ['./mitglieder-details.component.scss'],
  providers: [Unsubscribe]
})
export class MitgliederDetailsComponent  implements OnInit {

  data$!: Observable<{data: GetSpecificMitgliederResponse, instrumentDropdown: DropdownItem[], notenStimmeDropdown: DropdownItem[]}> | null;
  mitgliedsId!: string;

  constructor(
    private mitgliederService: MitgliederService,
    private route: ActivatedRoute,
    private dropdownService: DropdownService,
    private router: Router,
    private us: Unsubscribe,
    private modalCtrl: ModalController
  ) { }

  ngOnInit() {
    this.mitgliedsId = this.route.snapshot.params["mitgliedsId"];
    this.data$ = this.us.autoUnsubscribe(combineLatest([
      this.mitgliederService.getSpecificMitglied(this.mitgliedsId), 
      this.dropdownService.getDropdownElements('Instrument'),
      this.dropdownService.getDropdownElements('Notenstimme') 
    ])).pipe(map(([data, instrumentDropdown, notenStimmeDropdown]) => ({data, instrumentDropdown, notenStimmeDropdown})));
  }

  isActionSheetOpen = false;

  setOpen(isOpen: boolean) {
    this.isActionSheetOpen = isOpen;
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
  private deleteOrchesterMember(){
    if (!this.mitgliedsId) return;
    this.data$ = null;
    this.mitgliederService.deleteMitglied(this.mitgliedsId).subscribe(() => {
      this.router.navigate(['tabs', 'mitglieder']);
    })
  }

  private async openAdminUpdateOrchesterMemberModal() {
    const modal = await this.modalCtrl.create({
      component: MitgliedAdminUpdateModalComponent,
      componentProps: {"mitgliedsId": this.mitgliedsId}
    });
    modal.present();
    this.setOpen(false);

    const { data, role } = await modal.onWillDismiss();
    if (role === 'cancel') return;
  }

}
