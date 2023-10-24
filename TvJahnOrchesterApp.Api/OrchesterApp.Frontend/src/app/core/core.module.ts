import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreRoutingModule } from './core-routing.module';
import { TabsComponent } from './components/tabs/tabs.component';
import { IonicModule } from '@ionic/angular';
import { SettingsComponent } from './components/settings/settings.component';
import { UserFeedbackComponent } from './components/user-feedback/user-feedback.component';
import { UserHelpComponent } from './components/user-help/user-help.component';
import { SharedModule } from '../shared/shared.module';
import { AccountManagementComponent } from '../authentication/components/account-management/account-management.component';


@NgModule({
  declarations: [
    TabsComponent,
    SettingsComponent,
    UserFeedbackComponent,
    UserHelpComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    CoreRoutingModule, 
    IonicModule
  ],
})
export class CoreModule { }
