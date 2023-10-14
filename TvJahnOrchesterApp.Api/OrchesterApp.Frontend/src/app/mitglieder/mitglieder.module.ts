import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MitgliederRoutingModule } from './mitglieder-routing.module';
import { SharedModule } from '../shared/shared.module';
import { MitgliederListeComponent } from './components/mitglieder-liste/mitglieder-liste.component';
import { MitgliederManagementComponent } from './components/mitglieder-management/mitglieder-management.component';


@NgModule({
  declarations: [
    MitgliederListeComponent,
    MitgliederManagementComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    MitgliederRoutingModule
  ]
})
export class MitgliederModule { }
