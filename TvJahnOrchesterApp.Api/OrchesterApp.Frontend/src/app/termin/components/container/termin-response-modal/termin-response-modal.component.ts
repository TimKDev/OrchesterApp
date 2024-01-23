import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-termin-response-modal',
  templateUrl: './termin-response-modal.component.html',
  styleUrls: ['./termin-response-modal.component.scss'],
})
export class TerminResponseModalComponent  implements OnInit {

  constructor(
    private modalCtrl: ModalController,
  ) { }

  ngOnInit() {}

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

}
