import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpParams } from '@capacitor/core';
import { Observable } from 'rxjs';

export const BASE_PATH = 'https://localhost:44331/';

@Injectable({
  providedIn: 'root'
})
export class UnauthorizedHttpClientService {
  constructor(private http: HttpClient) { }

  get<T>(url: string, params?: HttpParams): Observable<T> {
    return this.http.get<T>(BASE_PATH + url, { params });
  }

  post<T>(url: string, body: any): Observable<T> {
    return this.http.post<T>(BASE_PATH + url, body);
  }

  put<T>(url: string, body: any): Observable<T> {
    return this.http.put<T>(BASE_PATH + url, body);
  }
}
