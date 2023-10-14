import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreRoutingModule } from './core-routing.module';
import { TabsComponent } from './components/tabs/tabs.component';
import { IonicModule } from '@ionic/angular';


@NgModule({
  declarations: [
    TabsComponent
  ],
  imports: [
    CommonModule,
    CoreRoutingModule, 
    IonicModule
  ],
})
export class CoreModule { }
