import { Component, OnInit } from '@angular/core';
import { LoadingController, ModalController } from '@ionic/angular';
import { GetAdminInfoResponse } from '../../../interfaces/get-admin-info-response';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AccountManagementService } from 'src/app/core/services/account-management.service';
import { Observable, catchError, tap } from 'rxjs';
import { GetAdminInfoDetailsResponse } from 'src/app/core/interfaces/get-admin-info-details-response';
import { ActivatedRoute } from '@angular/router';

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

}
