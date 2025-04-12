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
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    LoginComponent,
    RegistrationComponent,
    ShowHidePasswordComponent,
    VerifyEmailInfoComponent,
    EmailConfirmedComponent,
    ForgotPasswordComponent,
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
