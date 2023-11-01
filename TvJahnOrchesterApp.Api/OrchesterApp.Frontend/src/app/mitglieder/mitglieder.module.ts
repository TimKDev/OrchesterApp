import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MitgliederRoutingModule } from './mitglieder-routing.module';
import { SharedModule } from '../shared/shared.module';
import { MitgliederListeComponent } from './components/container/mitglieder-liste/mitglieder-liste.component';
import { IonicModule } from '@ionic/angular';
import { ContactItemComponent } from './components/presentational/contact-item/contact-item.component';


@NgModule({
  declarations: [
    MitgliederListeComponent,
    ContactItemComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    MitgliederRoutingModule,
    IonicModule
  ]
})
export class MitgliederModule { }
