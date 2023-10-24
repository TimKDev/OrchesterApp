import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { GetAdminInfoResponse } from '../../interfaces/get-admin-info-response';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss'],
})
export class AccountDetailsComponent {

  data!: GetAdminInfoResponse;

  constructor(private modalCtrl: ModalController) {}

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

}
