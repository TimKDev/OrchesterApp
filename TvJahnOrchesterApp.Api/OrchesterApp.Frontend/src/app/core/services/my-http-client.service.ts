import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpParams } from '@capacitor/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MyHttpClientService {
  private readonly BASE_PATH = 'https://localhost:44331/';

  constructor(private http: HttpClient) { }

  get<T>(url: string, params?: HttpParams): Observable<T>{
    return this.http.get<T>(this.BASE_PATH + url, {params});
  }

  post<T>(url: string, body: any): Observable<T>{
    return this.http.post<T>(this.BASE_PATH + url, body);
  }

  put<T>(url: string, body: any): Observable<T>{
    return this.http.put<T>(this.BASE_PATH + url, body);
  }
}
