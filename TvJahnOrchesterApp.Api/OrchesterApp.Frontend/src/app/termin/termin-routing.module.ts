import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TerminListeComponent } from './components/termin-liste/termin-liste.component';
import { TerminDetailsComponent } from './components/termin-details/termin-details.component';
import { TerminEinsatzplanComponent } from './components/termin-einsatzplan/termin-einsatzplan.component';
import { TerminReturnMessagesComponent } from './components/termin-return-messages/termin-return-messages.component';
import { TerminReturnMessageDetailsComponent } from './components/termin-return-message-details/termin-return-message-details.component';
import { TerminBesetzungComponent } from './components/termin-besetzung/termin-besetzung.component';

const routes: Routes = [
  {path: '', component: TerminListeComponent},
  {path: 'details/:terminId', component: TerminDetailsComponent},
  {path: 'einsatzplan/:terminId', component: TerminEinsatzplanComponent},
  {path: 'return-messages/:terminId', component: TerminReturnMessagesComponent},
  {path: 'return-messages/:terminId/details/:returnMessageId', component: TerminReturnMessageDetailsComponent},
  {path: 'besetzung/:terminId', component: TerminBesetzungComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TerminRoutingModule { }
