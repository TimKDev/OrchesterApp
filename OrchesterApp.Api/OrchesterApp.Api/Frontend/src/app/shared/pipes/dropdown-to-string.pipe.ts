import { Pipe, PipeTransform } from '@angular/core';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';

@Pipe({
  name: 'dropdownToString'
})
export class DropdownToStringPipe implements PipeTransform {

  transform(dropdownValues: number[], dropdownItems: DropdownItem[]): string {
    const selectedItems = dropdownItems.filter(item => dropdownValues.includes(item.value as number));
    const resultString = selectedItems.map(item => item.text).join(', ');

    return resultString;
  }

}
