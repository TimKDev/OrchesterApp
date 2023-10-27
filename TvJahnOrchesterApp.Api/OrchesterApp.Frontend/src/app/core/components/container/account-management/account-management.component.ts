import { Component, OnInit } from '@angular/core';
import { AuthHttpClientService } from 'src/app/core/services/auth-http-client.service';
import { ModalController } from '@ionic/angular';
import { AccountDetailsComponent } from '../account-details/account-details.component';
import { GetAdminInfoResponse } from '../../../interfaces/get-admin-info-response';
import { AccountManagementService } from 'src/app/core/services/account-management.service';

@Component({
  selector: 'app-account-management',
  templateUrl: './account-management.component.html',
  styleUrls: ['./account-management.component.scss'],
})
export class AccountManagementComponent  implements OnInit {

  constructor(
    private accountManagementService: AccountManagementService,
  ) { }

  data$ = this.accountManagementService.getManagementInfos();

  ngOnInit() {}
}
