import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { TerminDetailsResponse } from 'src/app/termin/interfaces/termin-details-response';

@Component({
  selector: 'app-update-termin-modal',
  templateUrl: './update-termin-modal.component.html',
  styleUrls: ['./update-termin-modal.component.scss'],
})
export class UpdateTerminModalComponent  implements OnInit {

  dataTermin!: TerminDetailsResponse;

  formGroup = this.formBuilder.group({
    terminName: ['', [Validators.required]],
    terminArt: [0, [Validators.required]],
    terminStatus: [0, [Validators.required]],
    terminDate: [null, [Validators.required]],
    startZeit: [null],
    endZeit: [null],
    straße: [''],
    hausnummer: [''],
    postleitzahl: [''],
    stadt: [''],
    zusatz: [''],
    latitude: [null as number | null],
    longitude: [null as number | null],
    noten: [] as number[],
    uniform: [] as number[],
    orchestermitgliedIds: null as string[] | null
  });

  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.formGroup.patchValue({
      ...this.dataTermin.termin, 
      terminDate: formatDate(this.dataTermin.termin.startZeit, 'yyyy-MM-dd', 'en_EN'),
      startZeit: formatDate(this.dataTermin.termin.startZeit, 'HH:mm', 'en_EN'),
      endZeit: formatDate(this.dataTermin.termin.endZeit, 'HH:mm', 'en_EN'),
    } as any);
  }

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  confirm() {
    let value = this.formGroup.getRawValue();
    let startZeit = new Date(value.terminDate!);
    let endZeit = new Date(value.terminDate!);
    if(value.startZeit){
      startZeit.setHours(value.startZeit[0] + value.startZeit[1]);
      startZeit.setMinutes(value.startZeit[3] + value.startZeit[4]);
    }
    if(value.endZeit){
      endZeit.setHours(value.endZeit[0] + value.endZeit[1]);
      endZeit.setMinutes(value.endZeit[3] + value.endZeit[4]);
    }
    return this.modalCtrl.dismiss({
      ...value, 
      startZeit: startZeit,
      endZeit: endZeit
    }, 'confirm');
  }
}
