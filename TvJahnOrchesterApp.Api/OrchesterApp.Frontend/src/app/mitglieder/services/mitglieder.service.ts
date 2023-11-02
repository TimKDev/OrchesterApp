import { Injectable } from '@angular/core';
import { AuthHttpClientService } from '../../core/services/auth-http-client.service'
import { GetAllMitgliederResponse } from '../interfaces/get-all-mitglieder-response';

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
}
