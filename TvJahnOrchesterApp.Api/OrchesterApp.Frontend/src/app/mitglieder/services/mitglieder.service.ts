import { Injectable } from '@angular/core';
import { AuthHttpClientService } from '../../core/services/auth-http-client.service'
import { GetAllMitgliederResponse } from '../interfaces/get-all-mitglieder-response';
import { GetSpecificMitgliederResponse } from '../interfaces/get-specific-mitglieder-response';

@Injectable({
  providedIn: 'root'
})
export class MitgliederService {

  constructor(
    private http: AuthHttpClientService
  ) { }

  public getAllMitglieder(){
    return this.http.get<GetAllMitgliederResponse[]>('api/orchester-mitglied/get-all');
  }

  public getSpecificMitglied(mitgliedsId: string ){
    return this.http.get<GetSpecificMitgliederResponse>(`api/orchester-mitglied/specific/${mitgliedsId}`);
  }
}
