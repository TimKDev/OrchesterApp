import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TerminRoutingModule } from './termin-routing.module';
import { SharedModule } from '../shared/shared.module';
import { TerminListeComponent } from './components/container/termin-liste/termin-liste.component';
import { IonicModule } from '@ionic/angular';
import { CreateTerminModalComponent } from './components/container/create-termin-modal/create-termin-modal.component';
import { TerminBesetzungComponent } from './components/container/termin-besetzung/termin-besetzung.component';
import { TerminDetailsComponent } from './components/container/termin-details/termin-details.component';
import { TerminEinsatzplanComponent } from './components/container/termin-einsatzplan/termin-einsatzplan.component';
import { TerminReturnMessageDetailsComponent } from './components/container/termin-return-message-details/termin-return-message-details.component';
import { TerminReturnMessagesComponent } from './components/container/termin-return-messages/termin-return-messages.component';
import { UpdateTerminEinsatzplanComponent } from './components/container/update-termin-einsatzplan/update-termin-einsatzplan.component';
import { UpdateTerminModalComponent } from './components/container/update-termin-modal/update-termin-modal.component';
import { UpdateTerminReturnMessageDetailsComponent } from './components/container/update-termin-return-message-details/update-termin-return-message-details.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TerminItemComponent } from './components/presentational/termin-item/termin-item.component';
import { CurrentTerminListeComponent } from './components/container/current-termin-liste/current-termin-liste.component';
import { HistoryTerminListeComponent } from './components/container/history-termin-liste/history-termin-liste.component';
import { TerminOverviewComponent } from './components/container/termin-overview/termin-overview.component';
import { SelectTerminsFromDatePipe } from './pipes/select-termins-from-date.pipe';


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
    UpdateTerminReturnMessageDetailsComponent,
    TerminItemComponent,
    CurrentTerminListeComponent,
    HistoryTerminListeComponent,
    TerminOverviewComponent,
    SelectTerminsFromDatePipe
  ],
  imports: [
    SharedModule,
    CommonModule,
    TerminRoutingModule,
    IonicModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class TerminModule { }
