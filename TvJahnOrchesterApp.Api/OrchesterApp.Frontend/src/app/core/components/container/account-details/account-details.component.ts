import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { GetAdminInfoResponse } from '../../../../authentication/interfaces/get-admin-info-response';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-account-details',
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss'],
})
export class AccountDetailsComponent implements OnInit {

  public readonly roles = [
    {value: "Admin", text: "Admin"},
    {value: "Musikalischer Leiter", text: "Musikalischer Leiter"},
    {value: "Vorstand", text: "Vorstand"},
  ];

  data!: GetAdminInfoResponse;
  roleFormGroup!: FormGroup;

  constructor(
    private modalCtrl: ModalController,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.roleFormGroup = this.fb.group({
      role: [this.data.roleNames],
    });
  }

  cancel() {
    return this.modalCtrl.dismiss(null, 'cancel');
  }

  changeRoles(){
    let test = this.role;
    debugger;
  }

  get role() {
    return this.roleFormGroup.get('role');
  }

}
