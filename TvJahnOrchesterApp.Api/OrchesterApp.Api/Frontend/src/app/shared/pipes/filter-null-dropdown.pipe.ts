import { Pipe, PipeTransform } from '@angular/core';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';

@Pipe({
  name: 'filterNullDropdown'
})
export class FilterNullDropdownPipe implements PipeTransform {

  transform(value: DropdownItem[]): DropdownItem[]{
    return value.filter(i => i.value !== null);
  }

}
