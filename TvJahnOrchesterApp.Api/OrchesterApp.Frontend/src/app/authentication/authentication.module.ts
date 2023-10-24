import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthenticationRoutingModule } from './authentication-routing.module';
import { LoginComponent } from './components/login/login.component';
import { IonicModule } from '@ionic/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RegistrationComponent } from './components/registration/registration.component';
import { ShowHidePasswordComponent } from './components/show-hide-password/show-hide-password.component';
import { VerifyEmailInfoComponent } from './components/verify-email-info/verify-email-info.component';
import { EmailConfirmedComponent } from './components/email-confirmed/email-confirmed.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { AccountManagementComponent } from './components/account-management/account-management.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    LoginComponent,
    RegistrationComponent,
    ShowHidePasswordComponent,
    VerifyEmailInfoComponent,
    EmailConfirmedComponent,
    ForgotPasswordComponent,
    AccountManagementComponent
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
