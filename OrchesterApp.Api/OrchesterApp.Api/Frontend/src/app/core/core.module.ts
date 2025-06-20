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
import { GetCardColorPipe } from './pipes/get-card-color.pipe';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SideMenuComponent } from './components/container/side-menu/side-menu.component';
import { AccountItemComponent } from './components/presentational/account-item/account-item.component';
import { ImpressumComponent } from './components/container/impressum/impressum.component';


@NgModule({
  declarations: [
    TabsComponent,
    SettingsComponent,
    UserFeedbackComponent,
    UserHelpComponent,
    AccountManagementComponent,
    AccountDetailsComponent,
    GetCardColorPipe,
    SideMenuComponent, 
    AccountItemComponent,
    ImpressumComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    CoreRoutingModule, 
    IonicModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [
    SideMenuComponent
  ]
})
export class CoreModule { }
