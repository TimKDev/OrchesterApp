import { Pipe, PipeTransform } from '@angular/core';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';

@Pipe({
  name: 'dropdown'
})
export class DropdownPipe implements PipeTransform {

  transform(value: number | undefined, items: DropdownItem[]): string{
    return items.find(i => i.value === value)?.text ?? 'unbekannt';
  }

}
