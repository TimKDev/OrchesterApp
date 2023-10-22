import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { EmailConfirmedComponent } from './components/email-confirmed/email-confirmed.component';
import { VerifyEmailInfoComponent } from './components/verify-email-info/verify-email-info.component';

const routes: Routes = [
  {path: '', component: LoginComponent},
  {path: 'registration', component: RegistrationComponent},
  {path: 'verify-email-info', component: VerifyEmailInfoComponent},
  {path: 'email-confirmed', component: EmailConfirmedComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthenticationRoutingModule { }
