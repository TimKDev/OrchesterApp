import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { IonicModule } from '@ionic/angular';
import { SideMenuComponent } from './components/side-menu/side-menu.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    ToolbarComponent,
    SideMenuComponent, 
  ],
  imports: [
    CommonModule, 
    RouterModule,
    IonicModule
  ],
  exports: [
    ToolbarComponent, 
    SideMenuComponent
  ]
})
export class SharedModule { }
