import { Injectable } from '@angular/core';
import { AuthHttpClientService } from 'src/app/core/services/auth-http-client.service';
import { GetAllTermineResponse } from '../interfaces/get-all-termine-response';
import { CreateTerminRequest } from '../interfaces/create-termin-request';
import { DropdownItem } from 'src/app/core/interfaces/dropdown-item';
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TerminService {

  private getAllTerminCache: GetAllTermineResponse[] | null = null; 
  private getAllTerminCacheLastUpdated: number | null = null;

  private getAllTerminHistoryCache: GetAllTermineResponse[] | null = null;
  private getAllTerminHistoryCacheLastUpdated: number | null = null;

  constructor(
    private http: AuthHttpClientService
  ) { }

  public getAllTermins(useCache = true){
    if(useCache && this.getAllTerminCache && this.getAllTerminCacheLastUpdated && this.getAllTerminCacheLastUpdated > (Date.now() - 1000 * 60 * 10)){
      return of(this.getAllTerminCache);
    }
    return this.http.get<GetAllTermineResponse[]>('api/termin/all').pipe(tap(terminResponse => {
      this.getAllTerminCache = terminResponse;
      this.getAllTerminCacheLastUpdated = Date.now();
    }));
  }

  public getAllTerminsHistory(useCache = true){
    if(useCache && this.getAllTerminHistoryCache && this.getAllTerminHistoryCacheLastUpdated && this.getAllTerminHistoryCacheLastUpdated > (Date.now() - 1000 * 60 * 10)){
      return of(this.getAllTerminHistoryCache);
    }
    return this.http.get<GetAllTermineResponse[]>('api/termin/all/history').pipe(tap(terminHistoryResponse => {
      this.getAllTerminHistoryCache = terminHistoryResponse;
      this.getAllTerminHistoryCacheLastUpdated = Date.now();
    }));
  }

  public createNewTermin(data: CreateTerminRequest){
    return this.http.post<void>('api/termin/create', data);
  }

  public getOrchesterMitgliedDropdownEntries(){
    return this.http.get<DropdownItem[]>('api/dropdown/orchester-mitglieder');
  }
}
