import { Component, OnInit } from '@angular/core';
import { AuthHttpClientService } from 'src/app/core/services/auth-http-client.service';
import { ModalController } from '@ionic/angular';
import { AccountDetailsComponent } from '../account-details/account-details.component';
import { GetAdminInfoResponse } from '../../../../authentication/interfaces/get-admin-info-response';

@Component({
  selector: 'app-account-management',
  templateUrl: './account-management.component.html',
  styleUrls: ['./account-management.component.scss'],
})
export class AccountManagementComponent  implements OnInit {

  constructor(
    private http: AuthHttpClientService,
  ) { }

  data$ = this.http.get<GetAdminInfoResponse[]>('api/authentication/user-admin-infos');

  ngOnInit() {}
}
