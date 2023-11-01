import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MitgliederListeComponent } from './components/container/mitglieder-liste/mitglieder-liste.component';

const routes: Routes = [
  {path: '', component: MitgliederListeComponent},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MitgliederRoutingModule { }
