import { Injectable } from '@angular/core';
import { AuthHttpClientService } from './auth-http-client.service';
import { GetAdminInfoDetailsResponse } from '../interfaces/get-admin-info-details-response';
import { GetAdminInfoResponse } from '../interfaces/get-admin-info-response';
import { UpdateRolesRequest } from '../interfaces/update-roles-request';

@Injectable({
  providedIn: 'root'
})
export class AccountManagementService {

  constructor(
    private http: AuthHttpClientService
  ) { }

  getManagementInfos() {
    return this.http.get<GetAdminInfoResponse[]>('api/authentication/user-admin-infos');
  }

  getManagementInfosDetails(orchesterMitgliedsId: string) {
    return this.http.get<GetAdminInfoDetailsResponse>(`api/authentication/user-admin-infos-details/${orchesterMitgliedsId}`);
  }

  updateRoles(request: UpdateRolesRequest){
    return this.http.put<void>('api/authentication/update-roles', request);
  }
}
