import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpParams } from '@capacitor/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UnauthorizedHttpClientService {
  constructor(private http: HttpClient) { }

  get<T>(url: string, params?: HttpParams): Observable<T> {
    return this.http.get<T>(environment.basePathBackend + url, { params });
  }

  post<T>(url: string, body: any): Observable<T> {
    debugger;
    return this.http.post<T>(environment.basePathBackend + url, body);
  }

  put<T>(url: string, body: any): Observable<T> {
    return this.http.put<T>(environment.basePathBackend + url, body);
  }
}
