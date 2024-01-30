import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { TerminDetailsResponse } from 'src/app/termin/interfaces/termin-details-response';

@Component({
  selector: 'app-termin-response-modal',
  templateUrl: './termin-response-modal.component.html',
  styleUrls: ['./termin-response-modal.component.scss'],
})
export class TerminResponseModalComponent  implements OnInit {

  dataTermin!: TerminDetailsResponse;

  formGroup = this.formBuilder.group({
    zugesagt: [''],
    kommentarZusage: [0]
  });

  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.formGroup.patchValue({
      ...this.dataTermin.terminRückmeldung, 
    } as any);
  }

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  confirm() {
    let value = this.formGroup.getRawValue();
    return this.modalCtrl.dismiss({
      ...value, 
      kommentar: value.kommentarZusage,
    }, 'confirm');
  }

}
