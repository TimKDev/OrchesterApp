import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { Observable, tap } from 'rxjs';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { GetSpecificMitgliederResponse } from 'src/app/mitglieder/interfaces/get-specific-mitglieder-response';
import { MitgliederService } from 'src/app/mitglieder/services/mitglieder.service';

@Component({
  selector: 'app-mitglied-update-modal',
  templateUrl: './mitglied-update-modal.component.html',
  styleUrls: ['./mitglied-update-modal.component.scss'],
  providers: [Unsubscribe]
})
export class MitgliedUpdateModalComponent  implements OnInit {

  mitgliedsId!: string;
  data$!: Observable<GetSpecificMitgliederResponse>;

  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder,
    private mitgliederService: MitgliederService,
    private us: Unsubscribe
  ) { }

  ngOnInit(){
    this.data$ = this.us.autoUnsubscribe(this.mitgliederService.getSpecificMitglied(this.mitgliedsId)).pipe(tap(dataRaw => {
      let data = {
        ...dataRaw, 
        geburtstag: dataRaw.geburtstag ? formatDate(dataRaw.geburtstag, 'yyyy-MM-dd', 'en') : null,
      }
      this.formGroup.patchValue(data);
      this.formGroup.patchValue(data.adresse);
    }));
  }

  formGroup = this.formBuilder.group({
    straße: '',
    hausnummer: '',
    postleitzahl: '',
    stadt: '',
    zusatz: '',
    geburtstag: '',
    telefonnummer: '',
    handynummer: '',
  });

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  confirm() {
    debugger;
    let value = this.formGroup.getRawValue();
    return this.modalCtrl.dismiss({...value, geburtstag: value.geburtstag === "" ? null : value.geburtstag}, 'confirm');
  }

}
