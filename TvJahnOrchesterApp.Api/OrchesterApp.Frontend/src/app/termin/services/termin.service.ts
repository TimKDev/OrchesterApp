import { Injectable } from '@angular/core';
import { AuthHttpClientService } from 'src/app/core/services/auth-http-client.service';
import { GetAllTermineResponse } from '../interfaces/get-all-termine-response';
import { CreateTerminRequest } from '../interfaces/create-termin-request';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';

@Injectable({
  providedIn: 'root'
})
export class TerminService {
  constructor(
    private http: AuthHttpClientService
  ) { }

  public getAllTermins(){
    return this.http.get<GetAllTermineResponse[]>('api/termin/all');
  }

  public getAllTerminsHistory(){
    return this.http.get<GetAllTermineResponse[]>('api/termin/all/history');
  }

  public createNewTermin(data: CreateTerminRequest){
    return this.http.post<void>('api/termin/create', data);
  }

  public getOrchesterMitgliedDropdownEntries(){
    return this.http.get<DropdownItem[]>('api/dropdown/orchester-mitglieder');
  }
}
