import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthenticationRoutingModule } from './authentication-routing.module';
import { LoginComponent } from './components/container/login/login.component';
import { IonicModule } from '@ionic/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RegistrationComponent } from './components/container/registration/registration.component';
import { ShowHidePasswordComponent } from './components/presentational/show-hide-password/show-hide-password.component';
import { VerifyEmailInfoComponent } from './components/container/verify-email-info/verify-email-info.component';
import { EmailConfirmedComponent } from './components/container/email-confirmed/email-confirmed.component';
import { ForgotPasswordComponent } from './components/container/forgot-password/forgot-password.component';
import { AccountManagementComponent } from './components/container/account-management/account-management.component';
import { SharedModule } from '../shared/shared.module';
import { AccountCardComponent } from './components/presentational/account-card/account-card.component';
import { AccountDetailsComponent } from './components/container/account-details/account-details.component';
import { GetCardColorPipe } from './pipes/get-card-color.pipe';
import { DataItemComponent } from './components/presentational/data-item/data-item.component';


@NgModule({
  declarations: [
    LoginComponent,
    RegistrationComponent,
    ShowHidePasswordComponent,
    VerifyEmailInfoComponent,
    EmailConfirmedComponent,
    ForgotPasswordComponent,
    AccountManagementComponent,
    AccountCardComponent,
    AccountDetailsComponent,
    GetCardColorPipe,
    DataItemComponent
  ],
  imports: [
    CommonModule,
    AuthenticationRoutingModule,
    IonicModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ]
})
export class AuthenticationModule { }
