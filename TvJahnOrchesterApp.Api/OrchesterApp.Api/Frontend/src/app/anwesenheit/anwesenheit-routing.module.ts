import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnwesenheitsListeComponent } from './components/anwesenheits-liste/anwesenheits-liste.component';

const routes: Routes = [
  {path: '', component: AnwesenheitsListeComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AnwesenheitRoutingModule { }
