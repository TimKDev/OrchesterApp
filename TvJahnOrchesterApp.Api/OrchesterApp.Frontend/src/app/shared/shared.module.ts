import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { IonicModule } from '@ionic/angular';
import { SideMenuComponent } from './components/side-menu/side-menu.component';
import { RouterModule } from '@angular/router';
import { LoaderComponent } from './components/loader/loader.component';



@NgModule({
  declarations: [
    ToolbarComponent,
    SideMenuComponent, 
    LoaderComponent
  ],
  imports: [
    CommonModule, 
    RouterModule,
    IonicModule
  ],
  exports: [
    ToolbarComponent, 
    SideMenuComponent,
    LoaderComponent
  ]
})
export class SharedModule { }
