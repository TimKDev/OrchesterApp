import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TerminRoutingModule } from './termin-routing.module';
import { SharedModule } from '../shared/shared.module';
import { TerminListeComponent } from './components/termin-liste/termin-liste.component';
import { IonicModule } from '@ionic/angular';
import { CreateTerminModalComponent } from './components/create-termin-modal/create-termin-modal.component';
import { TerminBesetzungComponent } from './components/termin-besetzung/termin-besetzung.component';
import { TerminDetailsComponent } from './components/termin-details/termin-details.component';
import { TerminEinsatzplanComponent } from './components/termin-einsatzplan/termin-einsatzplan.component';
import { TerminReturnMessageDetailsComponent } from './components/termin-return-message-details/termin-return-message-details.component';
import { TerminReturnMessagesComponent } from './components/termin-return-messages/termin-return-messages.component';
import { UpdateTerminEinsatzplanComponent } from './components/update-termin-einsatzplan/update-termin-einsatzplan.component';
import { UpdateTerminModalComponent } from './components/update-termin-modal/update-termin-modal.component';
import { UpdateTerminReturnMessageDetailsComponent } from './components/update-termin-return-message-details/update-termin-return-message-details.component';


@NgModule({
  declarations: [
    TerminListeComponent,
    CreateTerminModalComponent,
    TerminBesetzungComponent,
    TerminDetailsComponent,
    TerminEinsatzplanComponent,
    TerminReturnMessageDetailsComponent,
    TerminReturnMessagesComponent,
    UpdateTerminEinsatzplanComponent,
    UpdateTerminModalComponent,
    UpdateTerminReturnMessageDetailsComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    TerminRoutingModule,
    IonicModule
  ]
})
export class TerminModule { }
