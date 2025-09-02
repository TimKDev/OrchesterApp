import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { FileUploadService } from 'src/app/core/services/file-upload.service';
import { PhotoService } from 'src/app/core/services/photo.service';
import { TerminDetailsResponse } from 'src/app/termin/interfaces/termin-details-response';
import { FileNamePipe } from 'src/app/termin/pipes/file-name.pipe';

@Component({
  selector: 'app-update-termin-modal',
  templateUrl: './update-termin-modal.component.html',
  styleUrls: ['./update-termin-modal.component.scss'],
})
export class UpdateTerminModalComponent  implements OnInit {

  dataTermin!: TerminDetailsResponse;
  dokuments = [] as FileItem[];

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
    orchestermitgliedIds: null as string[] | null,
    weitereInformationen: [''],
    image: [''],
  });

  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder,
    private photoService: PhotoService,
    public fileUploadService: FileUploadService
  ) { }

  ngOnInit() {
    this.formGroup.patchValue({
      ...this.dataTermin.termin,
      terminDate: formatDate(this.dataTermin.termin.startZeit, 'yyyy-MM-dd', 'en_EN'),
      startZeit: formatDate(this.dataTermin.termin.startZeit, 'HH:mm', 'en_EN'),
      endZeit: formatDate(this.dataTermin.termin.endZeit, 'HH:mm', 'en_EN'),
    } as any);

    this.dokuments = this.dataTermin.termin.dokumente?.map(name => ({name})) ?? [];
  }

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  public uploadFile(){
    const input = document.createElement('input');
    input.type = 'file';
    input.multiple = true;
    input.accept = '.pdf,.doc,.docx,.jpg,.jpeg,.png,.txt';

    input.onchange = (event: any) => {
      const files = Array.from(event.target.files as FileList);
      this.dokuments = [...this.dokuments, ...files.map(f => ({
        name: this.fileUploadService.transformFileNameWithGuid(f.name),
        file: f
      } as FileItem))];
      this.formGroup.markAsDirty();
    };

    input.click();
  }

  public deleteFile(index: number){
    this.dokuments.splice(index, 1);
    this.formGroup.markAsDirty();
  }

  async uploadImage(){
    const imageAs64 = await this.photoService.getPhotoAsBase64();
    this.formGroup.patchValue({image: imageAs64});
    this.formGroup.markAsDirty();
  }

  deleteCurrentImage(){
    this.formGroup.patchValue({image: null});
    this.formGroup.markAsDirty();
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
      endZeit: endZeit,
      dokumente: this.dokuments
    }, 'confirm');
  }
}

export interface FileItem{
  name: string,
  file?: File
}
