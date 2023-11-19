import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { RouterModule } from '@angular/router';
import { LoaderComponent } from './components/loader/loader.component';
import { DataItemComponent } from './components/data-item/data-item.component';
import { DropdownPipe } from './pipes/dropdown.pipe';
import { FilterNullDropdownPipe } from './pipes/filter-null-dropdown.pipe';



@NgModule({
  declarations: [
    LoaderComponent,
    DataItemComponent,
    DropdownPipe,
    FilterNullDropdownPipe,
  ],
  imports: [
    CommonModule, 
    RouterModule,
    IonicModule
  ],
  exports: [
    LoaderComponent,
    DataItemComponent,
    DropdownPipe,
    FilterNullDropdownPipe
  ]
})
export class SharedModule { }
