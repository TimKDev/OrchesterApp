import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TerminRoutingModule } from './termin-routing.module';
import { SharedModule } from '../shared/shared.module';
import { TerminListeComponent } from './components/termin-liste/termin-liste.component';
import { IonicModule } from '@ionic/angular';


@NgModule({
  declarations: [
    TerminListeComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    TerminRoutingModule,
    IonicModule
  ]
})
export class TerminModule { }
