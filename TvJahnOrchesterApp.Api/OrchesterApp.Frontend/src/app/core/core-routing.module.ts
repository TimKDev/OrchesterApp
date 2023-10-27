import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TabsComponent } from './components/container/tabs/tabs.component';
import { SettingsComponent } from './components/container/settings/settings.component';
import { UserHelpComponent } from './components/container/user-help/user-help.component';
import { UserFeedbackComponent } from './components/container/user-feedback/user-feedback.component';
import { AccountManagementComponent } from './components/container/account-management/account-management.component';
import { AuthGuard } from './guards/auth.guard';
import { AccountDetailsComponent } from './components/container/account-details/account-details.component';

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
        path: 'settings',
        component: SettingsComponent
      },
      {path: 'account-management', component: AccountManagementComponent},
      {path: 'account-management/details/:orchesterMitgliedsId', component: AccountDetailsComponent},
      {
        path: 'help',
        component: UserHelpComponent
      },
      {
        path: 'feedback',
        component: UserFeedbackComponent
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
