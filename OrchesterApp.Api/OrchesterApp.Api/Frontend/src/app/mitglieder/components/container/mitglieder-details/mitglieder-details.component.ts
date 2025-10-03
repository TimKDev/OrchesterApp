import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertController, ModalController } from '@ionic/angular';
import { catchError, combineLatest, map, of, tap } from 'rxjs';
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
import { RolesService } from 'src/app/authentication/services/roles.service';
import { PhotoService } from 'src/app/core/services/photo.service';
import { SendCustomNotificationModalComponent } from '../send-custom-notification-modal/send-custom-notification-modal.component';
import { NotificationApiService } from 'src/app/core/services/notification-api.service';
import { SendCustomNotificationRequest } from 'src/app/core/interfaces/send-custom-notification-request.interface';

@Component({
  selector: 'app-mitglieder-details',
  templateUrl: './mitglieder-details.component.html',
  styleUrls: ['./mitglieder-details.component.scss'],
  providers: [Unsubscribe]
})
export class MitgliederDetailsComponent implements OnInit {

  data!: GetSpecificMitgliederResponse | null;
  mitgliedsId!: string;

  canUpdateMitglied = this.rolesService.isCurrentUserAdmin || this.rolesService.isCurrentUserVorstand;

  constructor(
    private mitgliederService: MitgliederService,
    private route: ActivatedRoute,
    private router: Router,
    private us: Unsubscribe,
    private modalCtrl: ModalController,
    private refreshService: RefreshService,
    private authenticationService: AuthenticationService,
    private rolesService: RolesService,
    public alertController: AlertController,
    private notificationApiService: NotificationApiService
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

  loadData(refreshEvent: any = null) {
    this.us.autoUnsubscribe(this.mitgliederService.getSpecificMitglied(this.mitgliedsId)).subscribe(res => {
      this.data = res;
      if(refreshEvent) refreshEvent.target.complete();
    });
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
      text: 'Benachrichtigung senden',
      data: {
        action: 'share',
      },
      handler: () => this.openSendCustomNotificationModal()
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
    this.data = null;
    this.refreshService.refreshComponent("MitgliederListeComponent");
    this.mitgliederService.deleteMitglied(this.mitgliedsId).subscribe(() => {
      this.router.navigate(['tabs', 'mitglieder']);
    })
  }

  private async openAdminUpdateOrchesterMemberModal() {
    const modal = await this.modalCtrl.create({
      component: MitgliedAdminUpdateModalComponent,
      componentProps: {
        "mitgliedsId": this.mitgliedsId,
        "dropdownItemsNotenstimme": this.data?.notenstimmeDropdownItems,
        "dropdownItemsInstruments": this.data?.instrumentDropdownItems,
        "dropdownItemsPosition": this.data?.positionDropdownItems,
        "dropdownItemsMitgliedsStatus": this.data?.mitgliedsStatusDropdownItems,
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
    this.refreshService.refreshComponent('Dashboard');
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

  public handleRefresh(event: any){
    this.loadData(event);
  }

  private updateData(data: any){
    this.refreshService.refreshComponent("MitgliederListeComponent");
    this.refreshService.refreshComponent('Dashboard');
    this.us.autoUnsubscribe(this.mitgliederService.updateSpecificMitglied({...data, id: this.mitgliedsId} as UpdateSpecificMitgliederRequest)).subscribe(() => {
      this.loadData();
    })
  }

  public async openSendCustomNotificationModal() {
    if (!this.data) return;
    
    const modal = await this.modalCtrl.create({
      component: SendCustomNotificationModalComponent,
      componentProps: {
        "mitgliedsId": this.mitgliedsId,
        "mitgliedsName": this.data.orchesterMitglied.vorname + ' ' + this.data.orchesterMitglied.nachname
      }
    });
    modal.present();
    this.setOpen(false);

    const { data, role } = await modal.onWillDismiss();
    if (role === 'cancel') return;
    this.sendCustomNotification(data);
  }

  private async sendCustomNotification(data: SendCustomNotificationRequest) {
    this.us.autoUnsubscribe(
      this.notificationApiService.sendCustomNotification(data).pipe(
        catchError(async (error) => {
          const alert = await this.alertController.create({
            header: 'Fehler',
            message: 'Nachricht konnte nicht gesendet werden.',
            buttons: ['OK']
          });
          await alert.present();
          return of(null);
        })
      )
    ).subscribe(async () => {
          const alert = await this.alertController.create({
            header: 'Erfolg',
            message: 'Nachricht wurde erfolgreich gesendet.',
            buttons: ['OK']
          });
          await alert.present();
        });
  }
}
