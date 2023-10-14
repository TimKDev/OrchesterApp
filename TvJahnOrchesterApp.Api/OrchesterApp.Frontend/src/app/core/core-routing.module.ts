import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TabsComponent } from './components/tabs/tabs.component';

const routes: Routes = [
  {
    path: 'tabs',
    component: TabsComponent,
    children: [
      {
        path: 'dashboard',
        loadChildren: () => import('../dashboard/dashboard.module').then(m => m.DashboardModule)
      },
      {
        path: 'termin',
        loadChildren: () => import('../termin/termin.module').then(m => m.TerminModule)
      },
      {
        path: 'mitglieder',
        loadChildren: () => import('../mitglieder/mitglieder.module').then(m => m.MitgliederModule)
      },
      {
        path: 'anwesenheit',
        loadChildren: () => import('../anwesenheit/anwesenheit.module').then(m => m.AnwesenheitModule)
      },
      {
        path: '',
        redirectTo: '/tabs/dashboard',
        pathMatch: 'full'
      }
    ]
  },
  {
    path: '',
    redirectTo: '/tabs/dashboard',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreRoutingModule { }
