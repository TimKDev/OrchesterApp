import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { RouterModule } from '@angular/router';
import { LoaderComponent } from './components/loader/loader.component';
import { DataItemComponent } from './components/data-item/data-item.component';
import { DropdownPipe } from './pipes/dropdown.pipe';



@NgModule({
  declarations: [
    LoaderComponent,
    DataItemComponent,
    DropdownPipe,
  ],
  imports: [
    CommonModule, 
    RouterModule,
    IonicModule
  ],
  exports: [
    LoaderComponent,
    DataItemComponent,
    DropdownPipe
  ]
})
export class SharedModule { }
