import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { Observable, combineLatest, map } from 'rxjs';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { DropdownService } from 'src/app/core/services/dropdown.service';
import { PhotoService } from 'src/app/core/services/photo.service';

@Component({
  selector: 'app-mitglied-create-modal',
  templateUrl: './mitglied-create-modal.component.html',
  styleUrls: ['./mitglied-create-modal.component.scss'],
  providers: [Unsubscribe]
})
export class MitgliedCreateModalComponent implements OnInit {

  data$!: Observable<{
    instrumentDropdown: DropdownItem[], notenStimmeDropdown: DropdownItem[], positionDropdown: DropdownItem[]
  }>;

  formGroup = this.formBuilder.group({
    vorname: ['', [Validators.required]],
    nachname: ['', [Validators.required]],
    straße: null as string | null,
    hausnummer: null as string | null,
    postleitzahl: null as string | null,
    stadt: null as string | null,
    zusatz: null as string | null,
    geburtstag: null as string | null,
    telefonnummer: null as string | null,
    handynummer: null as string | null,
    defaultInstrument: null as number | null,
    defaultNotenStimme: null as number | null,
    memberSince: null as string | null,
    position: [] as number[],
    image: ''
  });

  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder,
    private us: Unsubscribe,
    private dropdownService: DropdownService,
    private photoService: PhotoService
  ) { }

  ngOnInit() {
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
    let value = this.formGroup.getRawValue();
    return this.modalCtrl.dismiss({
      ...value,
      geburtstag: value.geburtstag ? new Date(value.geburtstag) : null,
      memberSince: value.memberSince ? new Date(value.memberSince) : null,
    }, 'confirm');
  }

  async uploadImage(){
    const imageAs64 = await this.photoService.getPhotoAsBase64();
    this.formGroup.patchValue({image: imageAs64});
    this.formGroup.markAsDirty();
  }

}
