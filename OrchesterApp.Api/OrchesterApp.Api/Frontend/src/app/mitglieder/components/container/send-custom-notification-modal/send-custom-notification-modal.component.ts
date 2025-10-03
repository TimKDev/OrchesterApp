import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-send-custom-notification-modal',
  templateUrl: './send-custom-notification-modal.component.html',
  styleUrls: ['./send-custom-notification-modal.component.scss'],
})
export class SendCustomNotificationModalComponent implements OnInit {

  mitgliedsId!: string;
  mitgliedsName!: string;

  formGroup = this.formBuilder.group({
    title: ['', [Validators.required]],
    message: ['', [Validators.required]],
    shouldEmailBeSend: [false]
  });

  constructor(
    private modalCtrl: ModalController,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
  }

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  confirm() {
    let value = this.formGroup.getRawValue();
    return this.modalCtrl.dismiss({
      ...value,
      orchestermitgliedIds: [this.mitgliedsId]
    }, 'confirm');
  }

}

