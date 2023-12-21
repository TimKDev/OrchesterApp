import { Pipe, PipeTransform } from '@angular/core';
import { TermineListData } from '../interfaces/termin-list-data-response';

@Pipe({
  name: 'selectTerminsFromDate'
})
export class SelectTerminsFromDatePipe implements PipeTransform {

  transform(termins: TermineListData[], selectedDate: any): TermineListData[] {
    return termins.filter(t => t.startZeit.toString().includes(selectedDate));
  }

}
