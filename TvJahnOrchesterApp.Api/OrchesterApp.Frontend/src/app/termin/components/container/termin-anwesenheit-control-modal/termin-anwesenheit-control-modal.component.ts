import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { TerminRückmeldungsTableEntry } from 'src/app/termin/interfaces/termin-response-response';

@Component({
  selector: 'app-termin-anwesenheit-control-modal',
  templateUrl: './termin-anwesenheit-control-modal.component.html',
  styleUrls: ['./termin-anwesenheit-control-modal.component.scss'],
})
export class TerminAnwesenheitControlModalComponent  implements OnInit {

  dataResponses!: TerminRückmeldungsTableEntry[];

  formGroup = this.formBuilder.array;

  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.formGroup.patchValue(this.dataResponses as any);
  }

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  confirm() {
    let value = this.formGroup.getRawValue();
    return this.modalCtrl.dismiss(value, 'confirm');
  }
}
