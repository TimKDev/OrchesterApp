import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { RouterModule } from '@angular/router';
import { LoaderComponent } from './components/loader/loader.component';



@NgModule({
  declarations: [
    LoaderComponent,
  ],
  imports: [
    CommonModule, 
    RouterModule,
    IonicModule
  ],
  exports: [
    LoaderComponent,
  ]
})
export class SharedModule { }
