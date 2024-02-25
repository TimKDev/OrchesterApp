import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { Observable, tap } from 'rxjs';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { PhotoService } from 'src/app/core/services/photo.service';
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
  data$!: Observable<GetSpecificMitgliederResponse>;
  dropdownItemsInstruments!: DropdownItem[];
  dropdownItemsNotenstimme!: DropdownItem[];
  dropdownItemsMitgliedsStatus!: DropdownItem[];
  dropdownItemsPosition!: DropdownItem[];

  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder,
    private mitgliederService: MitgliederService,
    private us: Unsubscribe,
    private photoService: PhotoService
  ) { }

  ngOnInit(){
    this.data$ = this.us.autoUnsubscribe(this.mitgliederService.getSpecificMitglied(this.mitgliedsId)).pipe(tap(dataRaw => {
      let data = {
        ...dataRaw.orchesterMitglied, 
        geburtstag: dataRaw.orchesterMitglied.geburtstag ? formatDate(dataRaw.orchesterMitglied.geburtstag, 'yyyy-MM-dd', 'en') : null,
        memberSince: dataRaw.orchesterMitglied.memberSince ? formatDate(dataRaw.orchesterMitglied.memberSince, 'yyyy-MM-dd', 'en') : null,
      }
      this.formGroup.patchValue(data);
      this.formGroup.patchValue(data.adresse);
    }));
  }

  formGroup = this.formBuilder.group({
    vorname: ['', [Validators.required]],
    nachname: ['', [Validators.required]],
    straße: '',
    hausnummer: '',
    postleitzahl: '',
    stadt: '',
    zusatz: '',
    geburtstag: '',
    telefonnummer: '',
    handynummer: '',
    defaultInstrument: 0,
    defaultNotenStimme: 0,
    memberSince: '',
    positions: [[] as number[]],
    orchesterMitgliedsStatus: 1,
    image: ''
  });

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  confirm() {
    let value = this.formGroup.getRawValue();
    return this.modalCtrl.dismiss({
      ...value, 
      geburtstag: value.geburtstag === "" ? null : value.geburtstag,
      memberSince: value.memberSince === "" ? null : value.memberSince
    }, 'confirm');
  }

  async uploadImage(){
    const imageAs64 = await this.photoService.getPhotoAsBase64();
    this.formGroup.patchValue({image: imageAs64});
    this.formGroup.markAsDirty();
  }

}
