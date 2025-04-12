import { Injectable } from '@angular/core';
import { AuthHttpClientService } from 'src/app/core/services/auth-http-client.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizedAuthServiceService {

  constructor(
    private authHttp: AuthHttpClientService
  ) { }

  public deleteOwnUser(){
    return this.authHttp.delete('api/authentication/delete-own-user');
  }
}
