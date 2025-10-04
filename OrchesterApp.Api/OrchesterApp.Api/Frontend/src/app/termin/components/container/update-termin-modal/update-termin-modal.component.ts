import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AlertController, ModalController } from '@ionic/angular';
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
    fristDays: [null as number | null],
    ersteWarnungVorFristDays: [null as number | null]
  });

  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder,
    public alertController: AlertController,
    private photoService: PhotoService,
    public fileUploadService: FileUploadService
  ) { }

  ngOnInit() {
    const fristDays = this.dataTermin.termin.frist ? this.parseTimeSpanToDays(this.dataTermin.termin.frist) : null;
    const ersteWarnungVorFristDays = this.dataTermin.termin.ersteWarnungVorFrist ? this.parseTimeSpanToDays(this.dataTermin.termin.ersteWarnungVorFrist) : null;
    
    this.formGroup.patchValue({
      ...this.dataTermin.termin,
      terminDate: formatDate(this.dataTermin.termin.startZeit, 'yyyy-MM-dd', 'en_EN'),
      startZeit: formatDate(this.dataTermin.termin.startZeit, 'HH:mm', 'en_EN'),
      endZeit: formatDate(this.dataTermin.termin.endZeit, 'HH:mm', 'en_EN'),
      fristDays: fristDays,
      ersteWarnungVorFristDays: ersteWarnungVorFristDays
    } as any);

    this.dokuments = this.dataTermin.termin.dokumente?.map(name => ({name})) ?? [];
  }

  private parseTimeSpanToDays(timeSpan: string): number | null {
    // Parse TimeSpan format "d.hh:mm:ss" or "hh:mm:ss"
    const parts = timeSpan.split('.');
    if (parts.length === 2) {
      // Format: "d.hh:mm:ss"
      return parseInt(parts[0], 10);
    }
    return null;
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

  async confirm() {
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

    const terminStartDate = new Date(startZeit);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    terminStartDate.setHours(0, 0, 0, 0);

    let shouldEmailBeSend = false;
    let shouldNotificationBeSend = false;
    if(this.shouldNotificationBeSend()){
      shouldEmailBeSend = terminStartDate >= today ? await this.askForEmailNotification() : false;
      shouldNotificationBeSend = true;
    }

    const frist = value.fristDays ? `${value.fristDays}.00:00:00` : undefined;
    const ersteWarnungVorFrist = value.ersteWarnungVorFristDays ? `${value.ersteWarnungVorFristDays}.00:00:00` : undefined;

    return this.modalCtrl.dismiss({
      ...value,
      startZeit: startZeit,
      endZeit: endZeit,
      dokumente: this.dokuments,
      shouldEmailBeSend: shouldEmailBeSend,
      shouldNotificationBeSend: shouldNotificationBeSend,
      frist: frist,
      ersteWarnungVorFrist: ersteWarnungVorFrist
    }, 'confirm');
  }

  private shouldNotificationBeSend(): boolean {
    const formValue = this.formGroup.getRawValue();
    const oldTermin = this.dataTermin.termin;

    if (formValue.terminStatus !== oldTermin.terminStatus) {
      return true;
    }

    if (formValue.straße !== oldTermin.straße ||
        formValue.hausnummer !== oldTermin.hausnummer ||
        formValue.postleitzahl !== oldTermin.postleitzahl ||
        formValue.stadt !== oldTermin.stadt ||
        formValue.zusatz !== oldTermin.zusatz) {
      return true;
    }

    const oldStartZeit = formatDate(oldTermin.startZeit, 'HH:mm', 'en_EN');
    if (formValue.startZeit !== oldStartZeit) {
      return true;
    }

    const oldEndZeit = formatDate(oldTermin.endZeit, 'HH:mm', 'en_EN');
    if (formValue.endZeit !== oldEndZeit) {
      return true;
    }

    const newNoten = Array.isArray(formValue.noten) ? formValue.noten : [];
    const oldNoten = Array.isArray(oldTermin.noten) ? oldTermin.noten : [];
    if (!this.arraysEqual(newNoten, oldNoten)) {
      return true;
    }

    const newUniform = Array.isArray(formValue.uniform) ? formValue.uniform : [];
    const oldUniform = Array.isArray(oldTermin.uniform) ? oldTermin.uniform : [];
    if (!this.arraysEqual(newUniform, oldUniform)) {
      return true;
    }

    return false;
  }

  private arraysEqual(arr1: number[], arr2: number[]): boolean {
    if (arr1.length !== arr2.length) {
      return false;
    }
    const sorted1 = [...arr1].sort();
    const sorted2 = [...arr2].sort();
    return sorted1.every((val, index) => val === sorted2[index]);
  }

  private async askForEmailNotification(): Promise<boolean> {
    const alert = await this.alertController.create({
      header: 'E-Mail Benachrichtigung',
      message: 'Möchten Sie die betroffenen Orchestermitglieder per E-Mail über diese Terminänderungen informieren?',
      buttons: [
        {
          text: 'Nein',
          role: 'cancel',
        },
        {
          text: 'Ja',
          role: 'confirm',
        },
      ]
    });
    
    await alert.present();
    const alertResult = await alert.onDidDismiss();
    return alertResult.role === 'confirm';
  }
}

export interface FileItem{
  name: string,
  file?: File
}
