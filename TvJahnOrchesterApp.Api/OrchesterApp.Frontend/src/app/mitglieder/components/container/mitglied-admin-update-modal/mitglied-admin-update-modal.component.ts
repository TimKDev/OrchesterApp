import { Component } from '@angular/core';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-mitglied-admin-update-modal',
  templateUrl: './mitglied-admin-update-modal.component.html',
  styleUrls: ['./mitglied-admin-update-modal.component.scss'],
})
export class MitgliedAdminUpdateModalComponent {

  name!: string;

  constructor(private modalCtrl: ModalController) {}

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  confirm() {
    return this.modalCtrl.dismiss(this.name, 'confirm');
  }

}
