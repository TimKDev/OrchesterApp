import { Pipe, PipeTransform } from '@angular/core';
import { GetAdminInfoResponse } from '../../authentication/interfaces/get-admin-info-response';

@Pipe({
  name: 'getCardColor'
})
export class GetCardColorPipe implements PipeTransform {

  transform(value: GetAdminInfoResponse): string {
    if(!value.email) return 'medium';
    return 'primary';
  }

}
