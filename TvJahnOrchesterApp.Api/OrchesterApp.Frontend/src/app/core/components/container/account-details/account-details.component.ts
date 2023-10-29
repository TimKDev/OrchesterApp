import { Component, OnInit } from '@angular/core';
import { AlertController, LoadingController } from '@ionic/angular';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AccountManagementService } from 'src/app/core/services/account-management.service';
import { Observable, catchError, tap } from 'rxjs';
import { GetAdminInfoDetailsResponse } from 'src/app/core/interfaces/get-admin-info-details-response';
import { ActivatedRoute } from '@angular/router';
import { confirmDialog } from 'src/app/core/helper/confirm';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss'],
})
export class AccountDetailsComponent implements OnInit {

  public readonly roles = [
    {value: "Admin", text: "Admin"},
    {value: "Musikalischer Leiter", text: "Musikalischer Leiter"},
    {value: "Vorstand", text: "Vorstand"},
  ];

  data$!: Observable<GetAdminInfoDetailsResponse>;
  roleFormGroup!: FormGroup;

  private orchesterMitgliedsId!: string;

  constructor(
    private fb: FormBuilder,
    private accountManagementService: AccountManagementService,
    private route: ActivatedRoute,
    private loadingController: LoadingController,
    private alertController: AlertController,
  ) {}

  ngOnInit(): void {
    this.orchesterMitgliedsId = this.route.snapshot.params["orchesterMitgliedsId"];
    this.data$ = this.accountManagementService.getManagementInfosDetails(this.orchesterMitgliedsId).pipe(
      tap(data => {
        this.roleFormGroup = this.fb.group({
          role: [data.roleNames],
        });
      })
    );
  }

  async changeRoles(email: string){
    if(!this.role?.value) return;
    const loading = await this.loadingController.create();
    await loading.present();
    this.accountManagementService.updateRoles({email, roleNames: this.role.value })
    .pipe(catchError(async () => await loading.dismiss()))
    .subscribe(async () => await loading.dismiss());
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
      text: 'Erneuere Registrierungsschlüssel',
      data: {
        action: 'share',
      },
      handler: () => {console.log("Registrierungsschlüssel")}
    },
    {
      text: 'Entferne Lockout',
      handler: () => {console.log("Entferne Lockout")}
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
  private async deleteUser(){
    
    // Modal Sicherheitsabfrage
    // Request auführen
    // Laden anzeigen
    // Auf Accountübersicht navigieren und die Daten für diese neu laden
  }

  private updateRegistrationKey(){
    // Request ausführen
    // data$ löschen => Zeigt Ladesymbol an
    // data$ dieses Accounts neu laden
  }

  private removeLockOut(){

  }

}
