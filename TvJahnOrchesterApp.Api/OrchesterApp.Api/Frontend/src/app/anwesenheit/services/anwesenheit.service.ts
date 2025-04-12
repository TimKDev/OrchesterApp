import { Injectable } from '@angular/core';
import { AuthHttpClientService } from 'src/app/core/services/auth-http-client.service';
import { AnwesenheitsListeGetResponseEntry } from '../interfaces/anwesenheits-liste-get-response';

@Injectable({
  providedIn: 'root'
})
export class AnwesenheitService {

  constructor(
    private http: AuthHttpClientService
  ) { }

  public getAnwesenheitsListeForYear(year: number){
    return this.http.get<AnwesenheitsListeGetResponseEntry[]>(`api/termin/anwesenheit/all/${year}`);
  }
}
