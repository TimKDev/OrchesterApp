import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MitgliederListeComponent } from './components/container/mitglieder-liste/mitglieder-liste.component';
import { MitgliederDetailsComponent } from './components/container/mitglieder-details/mitglieder-details.component';

const routes: Routes = [
  {path: '', component: MitgliederListeComponent},
  {path: 'details/:mitgliedsId', component: MitgliederDetailsComponent},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MitgliederRoutingModule { }
