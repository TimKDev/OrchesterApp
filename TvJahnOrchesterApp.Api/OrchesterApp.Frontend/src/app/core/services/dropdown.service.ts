import { Injectable } from '@angular/core';
import { AuthHttpClientService } from './auth-http-client.service';
import { DropdownNames } from '../types/dropdown-names';
import { DropdownItem } from '../interfaces/dropdown-item';
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DropdownService {

  private cache: {[key: string]: DropdownItem[]} = {};

  constructor(
    private http: AuthHttpClientService
  ) { }
  
  public getDropdownElements(dropdownName: DropdownNames){
    return this.cache[dropdownName] ? of(this.cache[dropdownName]) : this.http.get<DropdownItem[]>(`api/dropdown/${dropdownName}`).pipe(tap(dropdownItems => this.cache[dropdownName] = dropdownItems));
  }

}
