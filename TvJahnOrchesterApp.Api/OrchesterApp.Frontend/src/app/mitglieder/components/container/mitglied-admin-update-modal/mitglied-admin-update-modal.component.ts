import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { Observable, tap } from 'rxjs';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { GetSpecificMitgliederResponse } from 'src/app/mitglieder/interfaces/get-specific-mitglieder-response';
import { MitgliederService } from 'src/app/mitglieder/services/mitglieder.service';

@Component({
  selector: 'app-mitglied-admin-update-modal',
  templateUrl: './mitglied-admin-update-modal.component.html',
  styleUrls: ['./mitglied-admin-update-modal.component.scss'],
  providers: [Unsubscribe]
})
export class MitgliedAdminUpdateModalComponent implements OnInit{

  mitgliedsId!: string;
  data$!: Observable<GetSpecificMitgliederResponse>

  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder,
    private mitgliederService: MitgliederService,
    private us: Unsubscribe
  ) { }

  ngOnInit(){
    this.data$ = this.us.autoUnsubscribe(this.mitgliederService.getSpecificMitglied(this.mitgliedsId)).pipe(tap(data => {
      this.formGroup.patchValue(data);
      this.formGroup.patchValue(data.adresse);
    }));
  }

  formGroup = this.formBuilder.group({
    vorname: '',
    nachname: '',
    straße: '',
    hausnummer: '',
    postleitzahl: '',
    stadt: '',
    zusatz: '',
    geburtstag: new Date(),
    telefonnummer: '',
    handynummer: '',
    defaultInstrument: 0,
    defaultNotenStimme: 0,
    memberSince: new Date(),
  });

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  confirm() {
    return this.modalCtrl.dismiss(this.formGroup.getRawValue(), 'confirm');
  }

}
