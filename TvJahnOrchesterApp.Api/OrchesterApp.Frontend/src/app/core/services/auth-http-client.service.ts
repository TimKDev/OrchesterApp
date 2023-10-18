import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpParams } from '@capacitor/core';
import { Observable } from 'rxjs';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthHttpClientService {
  private readonly BASE_PATH = 'https://localhost:44331/';

  constructor(private http: HttpClient, private authService: AuthenticationService) { }

  get<T>(url: string, params?: HttpParams): Observable<T>{
    return this.http.get<T>(this.BASE_PATH + url, {params, headers: this.createAuthHeader()});
  }

  post<T>(url: string, body: any): Observable<T>{
    return this.http.post<T>(this.BASE_PATH + url, body, {headers: this.createAuthHeader()});
  }

  put<T>(url: string, body: any): Observable<T>{
    return this.http.put<T>(this.BASE_PATH + url, body, {headers: this.createAuthHeader()});
  }

  private createAuthHeader(){
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.authService.token}`
    });
  }
}
