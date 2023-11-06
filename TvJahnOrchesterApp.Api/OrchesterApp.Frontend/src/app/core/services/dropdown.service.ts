import { Injectable } from '@angular/core';
import { AuthHttpClientService } from './auth-http-client.service';
import { DropdownNames } from '../types/dropdown-names';
import { DropdownItem } from '../interfaces/dropdown-item';

@Injectable({
  providedIn: 'root'
})
export class DropdownService {

  constructor(
    private http: AuthHttpClientService
  ) { }
  
  public getDropdownElements(dropdownName: DropdownNames){
    return this.http.get<DropdownItem[]>(`api/dropdown/${dropdownName}`);
  }


}
