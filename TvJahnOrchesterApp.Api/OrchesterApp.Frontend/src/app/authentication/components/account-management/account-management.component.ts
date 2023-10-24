import { Component, OnInit } from '@angular/core';
import { AuthHttpClientService } from 'src/app/core/services/auth-http-client.service';
import { GetAdminInfoResponse } from '../../interfaces/get-admin-info-response';
import { ModalController } from '@ionic/angular';
import { AccountDetailsComponent } from '../account-details/account-details.component';

@Component({
  selector: 'app-account-management',
  templateUrl: './account-management.component.html',
  styleUrls: ['./account-management.component.scss'],
})
export class AccountManagementComponent  implements OnInit {

  constructor(
    private http: AuthHttpClientService,
    private modalCtrl: ModalController
  ) { }

  data$ = this.http.get<GetAdminInfoResponse[]>('api/authentication/user-admin-infos');

  ngOnInit() {}

  async openAccountInfo(data: GetAdminInfoResponse){
    const modal = await this.modalCtrl.create({
      component: AccountDetailsComponent,
      componentProps: { data }
    });
    modal.present();
    
  }
}
