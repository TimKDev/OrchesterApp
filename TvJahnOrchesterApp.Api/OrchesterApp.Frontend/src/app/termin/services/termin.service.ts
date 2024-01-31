import { Injectable } from '@angular/core';
import { AuthHttpClientService } from 'src/app/core/services/auth-http-client.service';
import { TerminListDataResponse } from '../interfaces/termin-list-data-response';
import { CreateTerminRequest } from '../interfaces/create-termin-request';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { of, tap } from 'rxjs';
import { TerminDetailsResponse } from '../interfaces/termin-details-response';
import { UpdateTerminRequest } from '../interfaces/update-termin-request';
import { UpdateTerminResponseRequest } from '../interfaces/update-termin-response-request';
import { TerminResponseResponse } from '../interfaces/termin-response-response';

@Injectable({
  providedIn: 'root'
})
export class TerminService {

  private getAllTerminCache: TerminListDataResponse | null = null; 
  private getAllTerminCacheLastUpdated: number | null = null;

  constructor(
    private http: AuthHttpClientService
  ) { }

  public getAllTermins(useCache = true){
    if(useCache && this.getAllTerminCache && this.getAllTerminCacheLastUpdated && this.getAllTerminCacheLastUpdated > (Date.now() - 1000 * 60 * 10)){
      return of(this.getAllTerminCache);
    }
    return this.http.get<TerminListDataResponse>('api/termin/all').pipe(tap(terminResponse => {
      this.getAllTerminCache = terminResponse;
      this.getAllTerminCacheLastUpdated = Date.now();
    }));
  }

  public createNewTermin(data: CreateTerminRequest){
    return this.http.post<void>('api/termin/create', data);
  }

  public getOrchesterMitgliedDropdownEntries(){
    return this.http.get<DropdownItem[]>('api/dropdown/orchester-mitglieder');
  }

  public getTerminDetails(terminId: string){
    return this.http.get<TerminDetailsResponse>(`api/termin/getById/${terminId}`);
  }

  public updateTerminDetails(data: UpdateTerminRequest){
    return this.http.put(`api/termin/update`, data);
  }

  public updateTerminResponse(data: UpdateTerminResponseRequest){
    return this.http.put('api/termin/rückmeldung', data);
  }

  public deleteTermin(terminId: string){
    return this.http.delete(`api/termin/delete/${terminId}`);
  }

  public getTerminResponses(terminId: string){
    return this.http.get<TerminResponseResponse>(`api/termin/rückmeldung/${terminId}`);
  }
}
