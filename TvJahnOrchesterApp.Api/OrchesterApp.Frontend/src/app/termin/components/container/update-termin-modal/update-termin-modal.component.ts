import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-update-termin-modal',
  templateUrl: './update-termin-modal.component.html',
  styleUrls: ['./update-termin-modal.component.scss'],
})
export class UpdateTerminModalComponent  implements OnInit {

  constructor(
    private modalCtrl: ModalController,
  ) { }

  ngOnInit() {}

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }
}
