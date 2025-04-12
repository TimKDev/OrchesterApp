import { Pipe, PipeTransform } from '@angular/core';
import { TermineListData } from '../interfaces/termin-list-data-response';

@Pipe({
  name: 'selectTerminsFromDate'
})
export class SelectTerminsFromDatePipe implements PipeTransform {

  transform(termins: TermineListData[], selectedDate: any): TermineListData[] {
    let targetDate = new Date(selectedDate);
    return termins.filter(t => targetDate.getDate() === t.startZeit.getDate() && targetDate.getMonth() === t.startZeit.getMonth() && targetDate.getFullYear() === t.startZeit.getFullYear());
  }

}
