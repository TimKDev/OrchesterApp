import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreRoutingModule } from './core-routing.module';
import { TabsComponent } from './components/container/tabs/tabs.component';
import { IonicModule } from '@ionic/angular';
import { SettingsComponent } from './components/container/settings/settings.component';
import { UserFeedbackComponent } from './components/container/user-feedback/user-feedback.component';
import { UserHelpComponent } from './components/container/user-help/user-help.component';
import { SharedModule } from '../shared/shared.module';
import { AccountDetailsComponent } from './components/container/account-details/account-details.component';
import { AccountManagementComponent } from './components/container/account-management/account-management.component';
import { AccountCardComponent } from './components/presentational/account-card/account-card.component';
import { DataItemComponent } from './components/presentational/data-item/data-item.component';
import { GetCardColorPipe } from './pipes/get-card-color.pipe';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    TabsComponent,
    SettingsComponent,
    UserFeedbackComponent,
    UserHelpComponent,
    AccountManagementComponent,
    AccountCardComponent,
    AccountDetailsComponent,
    GetCardColorPipe,
    DataItemComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    CoreRoutingModule, 
    IonicModule,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class CoreModule { }
