import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AnwesenheitRoutingModule } from './anwesenheit-routing.module';
import { IonicModule } from '@ionic/angular';
import { AnwesenheitsListeComponent } from './components/anwesenheits-liste/anwesenheits-liste.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    AnwesenheitsListeComponent
  ],
  imports: [
    SharedModule,
    IonicModule,
    CommonModule,
    AnwesenheitRoutingModule
  ]
})
export class AnwesenheitModule { }
