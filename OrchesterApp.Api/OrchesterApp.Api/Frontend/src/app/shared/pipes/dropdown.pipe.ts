import { Pipe, PipeTransform } from '@angular/core';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';

@Pipe({
  name: 'dropdown'
})
export class DropdownPipe implements PipeTransform {

  transform(value: number | number[] | undefined, items: DropdownItem[]): string{
    if(Array.isArray(value)){
      if(value.length === 0) return "keine"
      return value.map(v => items.find(i => i.value === v)?.text ?? 'unbekannt').join(", ");
    }
    return items.find(i => i.value === value)?.text ?? 'unbekannt';
  }

}
