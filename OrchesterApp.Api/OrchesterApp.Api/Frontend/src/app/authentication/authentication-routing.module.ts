import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/container/login/login.component';
import { RegistrationComponent } from './components/container/registration/registration.component';
import { EmailConfirmedComponent } from './components/container/email-confirmed/email-confirmed.component';
import { VerifyEmailInfoComponent } from './components/container/verify-email-info/verify-email-info.component';
import { ForgotPasswordComponent } from './components/container/forgot-password/forgot-password.component';

const routes: Routes = [
  {path: '', component: LoginComponent},
  {path: 'registration', component: RegistrationComponent},
  {path: 'verify-email-info', component: VerifyEmailInfoComponent},
  {path: 'email-confirmation', component: EmailConfirmedComponent},
  {path: 'reset-password', component: ForgotPasswordComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthenticationRoutingModule { }
