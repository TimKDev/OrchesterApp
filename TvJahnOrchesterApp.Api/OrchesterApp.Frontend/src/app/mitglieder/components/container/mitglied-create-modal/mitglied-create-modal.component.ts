import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { Observable, tap, combineLatest, map } from 'rxjs';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { DropdownService } from 'src/app/core/services/dropdown.service';
import { GetSpecificMitgliederResponse } from 'src/app/mitglieder/interfaces/get-specific-mitglieder-response';
import { MitgliederService } from 'src/app/mitglieder/services/mitglieder.service';

@Component({
  selector: 'app-mitglied-create-modal',
  templateUrl: './mitglied-create-modal.component.html',
  styleUrls: ['./mitglied-create-modal.component.scss'],
  providers: [Unsubscribe]
})
export class MitgliedCreateModalComponent  implements OnInit {

  data$!: Observable<{
    instrumentDropdown: DropdownItem[], notenStimmeDropdown: DropdownItem[], positionDropdown: DropdownItem[]
  }>;

  formGroup = this.formBuilder.group({
    vorname: '',
    nachname: '',
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
    registerKey: '',
    positions: [[] as number[]],
  });

  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder,
    private mitgliederService: MitgliederService,
    private us: Unsubscribe,
    private dropdownService: DropdownService
  ) { }

  ngOnInit(){
    this.loadData();
  }

  

  loadData() {
    this.data$ = this.us.autoUnsubscribe(combineLatest([
      this.dropdownService.getDropdownElements('Instrument'),
      this.dropdownService.getDropdownElements('Notenstimme'),
      this.dropdownService.getDropdownElements('Position')
    ])).pipe(map(([instrumentDropdown, notenStimmeDropdown, positionDropdown]) => ({ instrumentDropdown, notenStimmeDropdown, positionDropdown })));
    }

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  confirm() {
    return this.modalCtrl.dismiss(this.formGroup.getRawValue(), 'confirm');
  }

}
