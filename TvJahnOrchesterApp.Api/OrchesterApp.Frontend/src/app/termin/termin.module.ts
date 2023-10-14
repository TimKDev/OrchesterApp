import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TerminRoutingModule } from './termin-routing.module';
import { SharedModule } from '../shared/shared.module';
import { TerminListeComponent } from './components/termin-liste/termin-liste.component';


@NgModule({
  declarations: [
    TerminListeComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    TerminRoutingModule
  ]
})
export class TerminModule { }
