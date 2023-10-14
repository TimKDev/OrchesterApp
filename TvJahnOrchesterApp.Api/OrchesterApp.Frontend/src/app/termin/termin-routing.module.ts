import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TerminListeComponent } from './components/termin-liste/termin-liste.component';

const routes: Routes = [
  {path: '', component: TerminListeComponent}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TerminRoutingModule { }
