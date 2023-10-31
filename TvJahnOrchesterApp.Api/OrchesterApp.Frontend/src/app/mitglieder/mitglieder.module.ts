import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MitgliederRoutingModule } from './mitglieder-routing.module';
import { SharedModule } from '../shared/shared.module';
import { MitgliederListeComponent } from './components/mitglieder-liste/mitglieder-liste.component';
import { IonicModule } from '@ionic/angular';


@NgModule({
  declarations: [
    MitgliederListeComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    MitgliederRoutingModule,
    IonicModule
  ]
})
export class MitgliederModule { }
