import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MitgliederListeComponent } from './components/mitglieder-liste/mitglieder-liste.component';
import { MitgliederManagementComponent } from './components/mitglieder-management/mitglieder-management.component';

const routes: Routes = [
  {path: '', component: MitgliederListeComponent},
  {path: 'management', component: MitgliederManagementComponent}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MitgliederRoutingModule { }
