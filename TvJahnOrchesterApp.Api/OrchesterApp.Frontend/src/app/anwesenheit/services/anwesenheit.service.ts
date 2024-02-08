import { Injectable } from '@angular/core';
import { AuthHttpClientService } from 'src/app/core/services/auth-http-client.service';

@Injectable({
  providedIn: 'root'
})
export class AnwesenheitService {

  constructor(
    private http: AuthHttpClientService
  ) { }
}
