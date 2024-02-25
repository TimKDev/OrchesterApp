import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { Observable, combineLatest, map } from 'rxjs';
import { Unsubscribe } from 'src/app/core/helper/unsubscribe';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { DropdownService } from 'src/app/core/services/dropdown.service';
import { TerminService } from '../../../services/termin.service';
import { PhotoService } from 'src/app/core/services/photo.service';

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
    terminArt: [null, [Validators.required]],
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
    orchestermitgliedIds: null as string[] | null,
    weitereInformationen: [''],
    image: ['']
  });

  terminOnlyForSpecificMembers = false;
  
  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder,
    private us: Unsubscribe,
    private dropdownService: DropdownService,
    private terminService: TerminService,
    private photoService: PhotoService
  ) { }

  ngOnInit() {
    this.data$ = this.us.autoUnsubscribe(combineLatest([
      this.dropdownService.getDropdownElements('TerminArten'),
      this.dropdownService.getDropdownElements('Noten'),
      this.dropdownService.getDropdownElements('Uniform'),
      this.terminService.getOrchesterMitgliedDropdownEntries()
    ])).pipe(map(([terminArtDropdown, notenDropdown, uniformDropdown, orchesterMitgliederDropdown]) => ({ 
      terminArtDropdown, 
      notenDropdown: notenDropdown.filter(d => d.value !== null), 
      uniformDropdown: uniformDropdown.filter(d => d.value !== null), 
      orchesterMitgliederDropdown 
    })));
  }

  clickedCheckbox(){
    this.terminOnlyForSpecificMembers = !this.terminOnlyForSpecificMembers;
    if(!this.terminOnlyForSpecificMembers){
      this.formGroup.get('orchestermitgliedIds')?.setValue(null);
    }
  }

  async uploadImage(){
    const imageAs64 = await this.photoService.getPhotoAsBase64();
    this.formGroup.patchValue({image: imageAs64});
    this.formGroup.markAsDirty();
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
    else{
      endZeit.setHours(23);
      endZeit.setMinutes(59);
    }
    return this.modalCtrl.dismiss({
      ...value, 
      startZeit: startZeit,
      endZeit: endZeit
    }, 'confirm');
  }

}
