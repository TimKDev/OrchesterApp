import { Injectable } from '@angular/core';
import { Preferences } from '@capacitor/preferences';
import { BehaviorSubject, Observable, switchMap, from, tap, combineLatest } from 'rxjs';
import { MyHttpClientService } from 'src/app/core/services/my-http-client.service';
import { LoginRequest } from '../interfaces/login-request';
import { LoginResponse } from '../interfaces/login-response';

const TOKEN_KEY = 'token';
const REFRESH_TOKEN_KEY = 'refresh-token';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  isAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  token = '';
  refreshToken = '';

  constructor(private http: MyHttpClientService) {
    this.loadToken();
  }

  async loadToken() {
    const token = await Preferences.get({ key: TOKEN_KEY });
    const refreshToken = await Preferences.get({ key: REFRESH_TOKEN_KEY });
    if (token && token.value && refreshToken && refreshToken.value) {
      this.token = token.value;
      this.refreshToken = refreshToken.value;
      this.isAuthenticated.next(true);
    } else {
      this.isAuthenticated.next(false);
    }
  }

  login(credentials: LoginRequest): Observable<unknown> {
    return this.http.post<LoginResponse>('api/authentication/login', credentials).pipe(
      switchMap((response) => {
        return combineLatest([
          from(Preferences.set({ key: TOKEN_KEY, value: response.token })),
          from(Preferences.set({ key: REFRESH_TOKEN_KEY, value: response.refreshToken }))
        ]);
      }),
      tap((_) => {
        this.isAuthenticated.next(true);
      })
    );
  }

  async logout(){
    this.isAuthenticated.next(false);
    await Preferences.remove({ key: TOKEN_KEY });
    await Preferences.remove({ key: REFRESH_TOKEN_KEY});
  }
}
