import { Injectable } from '@angular/core';
import { AuthHttpClientService } from '../../core/services/auth-http-client.service'
import { GetAllMitgliederResponse } from '../interfaces/get-all-mitglieder-response';
import { GetSpecificMitgliederResponse } from '../interfaces/get-specific-mitglieder-response';
import { UpdateAdminSpecificMitgliederRequest } from '../interfaces/update-admin-specific-mitglieder-request';
import { UpdateSpecificMitgliederRequest } from '../interfaces/update-specific-mitglieder-request';
import { CreateMitgliedRequest } from '../interfaces/create-mitglied-request';

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

  public deleteMitglied(mitgliedsId: string){
    return this.http.delete(`api/orchester-mitglied/${mitgliedsId}`);
  }

  public updateAdminSpecificMitglied(updateRequest: UpdateAdminSpecificMitgliederRequest){
    return this.http.put<void>(`api/orchester-mitglied/admin/specific`, updateRequest);
  }

  public updateSpecificMitglied(updateRequest: UpdateSpecificMitgliederRequest){
    return this.http.put<void>(`api/orchester-mitglied/specific`, updateRequest);
  }

  public createNewMitglied(createRequest: CreateMitgliedRequest){
    return this.http.post<void>('api/orchester-mitglied', createRequest);
  }
}
