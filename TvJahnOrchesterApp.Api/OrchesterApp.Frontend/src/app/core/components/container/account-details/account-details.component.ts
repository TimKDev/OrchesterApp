import { Component, OnInit } from '@angular/core';
import { AlertController, LoadingController } from '@ionic/angular';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AccountManagementService } from 'src/app/core/services/account-management.service';
import { Observable, catchError, tap } from 'rxjs';
import { GetAdminInfoDetailsResponse } from 'src/app/core/interfaces/get-admin-info-details-response';
import { ActivatedRoute, Router } from '@angular/router';
import { confirmDialog } from 'src/app/core/helper/confirm';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { CLIENT_URI_REGISTRATION } from 'src/app/authentication/services/authentication.service';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss'],
  providers: [Unsubscribe]
})
export class AccountDetailsComponent {

  public readonly roles = [
    { value: "Admin", text: "Admin" },
    { value: "Musikalischer Leiter", text: "Musikalischer Leiter" },
    { value: "Vorstand", text: "Vorstand" },
  ];

  data$!: Observable<GetAdminInfoDetailsResponse> | null;
  roleFormGroup!: FormGroup;
  refreshing = false;

  private orchesterMitgliedsId!: string;
  private userId!: string | null;

  constructor(
    private fb: FormBuilder,
    private accountManagementService: AccountManagementService,
    private route: ActivatedRoute,
    private loadingController: LoadingController,
    public alertController: AlertController,
    private router: Router,
    private us: Unsubscribe 
  ) {
  }

  ionViewWillEnter() {
    this.orchesterMitgliedsId = this.route.snapshot.params["orchesterMitgliedsId"];
    this.data$ = this.getData();
  }

  ionViewDidLeave(){
    this.us.unsubscribe();
  }

  async changeRoles(email: string) {
    if (!this.role?.value) return;
    const loading = await this.loadingController.create();
    await loading.present();
    this.accountManagementService.updateRoles({ email, roleNames: this.role.value })
      .pipe(catchError(async () => await loading.dismiss()))
      .subscribe(async () => await loading.dismiss());
  }

  private getData() {
    return this.us.autoUnsubscribe(this.accountManagementService.getManagementInfosDetails(this.orchesterMitgliedsId).pipe(
      tap(data => {
        this.userId = data.userId;
        this.roleFormGroup = this.fb.group({
          role: [data.roleNames],
        });
      })
    ));
  }

  get role() {
    return this.roleFormGroup.get('role');
  }

  isActionSheetOpen = false;

  setOpen(isOpen: boolean) {
    this.isActionSheetOpen = isOpen;
  }

  public actionSheetButtons = [
    {
      text: 'Lösche User',
      role: 'destructive',
      handler: () => this.deleteUser()

    },
    {
      text: 'Entferne Lockout',
      handler: () => this.removeLockOut()
    },
    {
      text: 'Zurück',
      role: 'cancel',
      data: {
        action: 'cancel',
      },
    },
  ];

  public actionSheetButtonsWithoutUser = [
    {
      text: 'Sende Registrierungsmail',
      data: {
        action: 'share',
      },
      handler: () => this.sendRegistrationMail()
    },
    {
      text: 'Zurück',
      role: 'cancel',
      data: {
        action: 'cancel',
      },
    },
  ];

  @confirmDialog("Achtung", "Möchten sie diesen User wirklich löschen? Diese Operation kann nicht rückgängig gemacht werden.")
  private async deleteUser() {
    if (!this.userId) return;
    this.data$ = null;
    this.accountManagementService.deleteUser(this.userId).subscribe(() => {
      this.router.navigate(['tabs', 'account-management']);
    })
  }

  private async sendRegistrationMail() {
    const alert = await this.alertController.create({
      header: 'Versende Registrierungmail',
      inputs: [{
        placeholder: 'E-Mail',
        type: 'text',
      }],
      buttons: [{
        text: 'Abbrechen',
        role: 'cancel',
      },
      {
        text: 'Senden',
        role: 'confirm',
      }],
    });

    await alert.present();
    let alertResult = await alert.onDidDismiss();
    if (alertResult.role === 'confirm') {
      this.data$ = null;
      this.accountManagementService.sendRegistrationMail(
        { 
          email: alertResult.data.values["0"], 
          orchesterMitgliedsId: this.orchesterMitgliedsId,
          clientUri: CLIENT_URI_REGISTRATION
        }).subscribe(() => {
        this.data$ = this.getData();
      })
    }
  }

  private removeLockOut() {
    if (!this.userId) return;
    this.data$ = null;
    this.accountManagementService.removeLockout({ userId: this.userId }).subscribe(() => {
      this.data$ = this.getData();
    });
  }

  handleRefresh(event: any) {
    this.refreshing = true;
    this.data$ = this.getData().pipe(tap(() => {
      event.target.complete();
      this.refreshing = false;
    }));
  }

}
