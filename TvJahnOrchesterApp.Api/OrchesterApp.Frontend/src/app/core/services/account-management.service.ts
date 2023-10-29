import { Injectable } from '@angular/core';
import { AuthHttpClientService } from './auth-http-client.service';
import { GetAdminInfoDetailsResponse } from '../interfaces/get-admin-info-details-response';
import { GetAdminInfoResponse } from '../interfaces/get-admin-info-response';
import { UpdateRolesRequest } from '../interfaces/update-roles-request';
import { UpdateRegistrationKeyRequest } from '../interfaces/update-registration-key-request';
import { RemoveLockOutRequest } from '../interfaces/remove-lock-out-request';

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

  deleteUser(userId: string){
    return this.http.delete<void>(`api/authentication/delete-user/${userId}`);
  }

  removeLockout(request: RemoveLockOutRequest){
    return this.http.post<void>('api/authentication/remove-user-locked-out', request);
  }

  updateRegistrationKey(request: UpdateRegistrationKeyRequest){
    return this.http.post<void>('api/authentication/add-registration-key', request);
  }
}
