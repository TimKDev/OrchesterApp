import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpParams } from '@capacitor/core';
import { Observable } from 'rxjs';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthHttpClientService {
  constructor(private http: HttpClient, private authService: AuthenticationService) { }

  get<T>(url: string, params?: HttpParams): Observable<T>{
    return this.http.get<T>(environment.basePathBackend + url, {params, headers: this.createAuthHeader()});
  }

  post<T>(url: string, body: unknown): Observable<T>{
    return this.http.post<T>(environment.basePathBackend + url, body, {headers: this.createAuthHeader()});
  }

  postFile<T>(url: string, body: FormData): Observable<T>{
    return this.http.post<T>(environment.basePathBackend + url, body, {headers: this.createAuthHeaderForFiles()});
  }

  put<T>(url: string, body: any): Observable<T>{
    return this.http.put<T>(environment.basePathBackend + url, body, {headers: this.createAuthHeader()});
  }

  delete<T>(url: string){
    return this.http.delete<T>(environment.basePathBackend + url, {headers: this.createAuthHeader()});
  }

  private createAuthHeader(){
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.authService.token}`
    });
  }

  private createAuthHeaderForFiles(){
    return new HttpHeaders({
      'Authorization': `Bearer ${this.authService.token}`
    });
  }
}
