import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { TerminRückmeldungsTableEntry } from 'src/app/termin/interfaces/termin-response-response';

@Component({
  selector: 'app-termin-return-message-details',
  templateUrl: './termin-return-message-details.component.html',
  styleUrls: ['./termin-return-message-details.component.scss'],
})
export class TerminReturnMessageDetailsComponent  implements OnInit {

  public response!: TerminRückmeldungsTableEntry;
  public responseDropdownValues!: DropdownItem[];
  public lastResponse?: Date;

  formGroup = this.formBuilder.group({
    orchesterMitgliedsId: [''],
    istAnwesend: [false], 
    kommentarAnwesenheit: [null],
    zugesagt: [0], 
    kommentarZusage: [null], 
  });

  constructor(
    private formBuilder: FormBuilder,
    private modalCtrl: ModalController,
  ) { }

  ngOnInit() {
    this.formGroup.patchValue({
      ...this.response, 
    } as any);
  }

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  confirm() {
    let value = this.formGroup.getRawValue();
    return this.modalCtrl.dismiss(value, 'confirm');
  }

}
