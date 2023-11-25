import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { Observable, combineLatest, map } from 'rxjs';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { DropdownService } from 'src/app/core/services/dropdown.service';
import { TerminService } from '../../services/termin.service';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-create-termin-modal',
  templateUrl: './create-termin-modal.component.html',
  styleUrls: ['./create-termin-modal.component.scss'],
  providers: [Unsubscribe]
})
export class CreateTerminModalComponent  implements OnInit {

  data$!: Observable<{
    terminArtDropdown: DropdownItem[], notenDropdown: DropdownItem[], uniformDropdown: DropdownItem[], orchesterMitgliederDropdown: DropdownItem[]
  }>;

  formGroup = this.formBuilder.group({
    name: ['', [Validators.required]],
    terminArt: [0, [Validators.required]],
    startZeit: [null, [Validators.required]],
    endZeit: [null, [Validators.required]],
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
    private formBuilder: FormBuilder,
    private us: Unsubscribe,
    private dropdownService: DropdownService,
    private terminService: TerminService
  ) { }

  ngOnInit() {
    this.data$ = this.us.autoUnsubscribe(combineLatest([
      this.dropdownService.getDropdownElements('TerminArten'),
      this.dropdownService.getDropdownElements('Noten'),
      this.dropdownService.getDropdownElements('Uniform'),
      this.terminService.getOrchesterMitgliedDropdownEntries()
    ])).pipe(map(([terminArtDropdown, notenDropdown, uniformDropdown, orchesterMitgliederDropdown]) => ({ terminArtDropdown, notenDropdown, uniformDropdown, orchesterMitgliederDropdown })));
  }

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  confirm() {
    // let value = this.formGroup.getRawValue();
    // return this.modalCtrl.dismiss({
    //   ...value, 
    //   geburtstag: value.geburtstag ? new Date(value.geburtstag) : null, 
    //   memberSince: value.memberSince ? new Date(value.memberSince) : null, 
    // }, 'confirm');
  }

}
