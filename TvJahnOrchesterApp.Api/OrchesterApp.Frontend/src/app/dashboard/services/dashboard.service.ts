import { Injectable } from '@angular/core';
import { AuthHttpClientService } from 'src/app/core/services/auth-http-client.service';
import { DashboardGetResponse } from '../interfaces/dashboard-get-response';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor(
    private http: AuthHttpClientService
  ) { }

  public getDashboardData(){
    return this.http.get<DashboardGetResponse>('api/dashboard');
  }
}
