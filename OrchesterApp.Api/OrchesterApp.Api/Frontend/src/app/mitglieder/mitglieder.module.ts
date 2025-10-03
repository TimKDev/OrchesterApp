import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MitgliederRoutingModule } from './mitglieder-routing.module';
import { SharedModule } from '../shared/shared.module';
import { MitgliederListeComponent } from './components/container/mitglieder-liste/mitglieder-liste.component';
import { IonicModule } from '@ionic/angular';
import { ContactItemComponent } from './components/presentational/contact-item/contact-item.component';
import { MitgliederDetailsComponent } from './components/container/mitglieder-details/mitglieder-details.component';
import { MitgliedAdminUpdateModalComponent } from './components/container/mitglied-admin-update-modal/mitglied-admin-update-modal.component';
import { MitgliedCreateModalComponent } from './components/container/mitglied-create-modal/mitglied-create-modal.component';
import { MitgliedUpdateModalComponent } from './components/container/mitglied-update-modal/mitglied-update-modal.component';
import { SendCustomNotificationModalComponent } from './components/container/send-custom-notification-modal/send-custom-notification-modal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    MitgliederListeComponent,
    ContactItemComponent,
    MitgliederDetailsComponent,
    MitgliedAdminUpdateModalComponent,
    MitgliedCreateModalComponent,
    MitgliedUpdateModalComponent,
    SendCustomNotificationModalComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    MitgliederRoutingModule,
    IonicModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class MitgliederModule { }
