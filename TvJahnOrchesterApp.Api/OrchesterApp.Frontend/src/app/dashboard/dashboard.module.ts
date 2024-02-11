import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { IonicModule } from '@ionic/angular';
import { FormsModule } from '@angular/forms';
import { DashboardComponent } from './components/container/dashboard/dashboard.component';
import { SharedModule } from '../shared/shared.module';
import { DashboardTerminItemComponent } from './components/presentational/dashboard-termin-item/dashboard-termin-item.component';
import { DashboardBirthdayItemComponent } from './components/presentational/dashboard-birthday-item/dashboard-birthday-item.component';


@NgModule({
  declarations: [
    DashboardComponent,
    DashboardTerminItemComponent,
    DashboardBirthdayItemComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    IonicModule, 
    FormsModule,
    SharedModule
  ]
})
export class DashboardModule { }
