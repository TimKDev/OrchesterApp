import { Injectable } from '@angular/core';
import { Preferences } from '@capacitor/preferences';
import { BehaviorSubject, tap } from 'rxjs';
import { LoginRequest } from '../interfaces/login-request';
import { LoginResponse } from '../interfaces/login-response';
import { UnauthorizedHttpClientService } from 'src/app/core/services/unauthorized-http-client.service';

export const TOKEN_KEY = 'token';
export const REFRESH_TOKEN_KEY = 'refresh-token';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  // isAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  token: string | null = null;
  refreshToken: string | null = null;

  constructor(
    private http: UnauthorizedHttpClientService,
  ) {
  }

  async loadTokensFromStorage() {
    const token = await Preferences.get({ key: TOKEN_KEY });
    const refreshToken = await Preferences.get({ key: REFRESH_TOKEN_KEY });
    this.token = token.value;
    this.refreshToken = refreshToken.value;
    return 
  }

  login(credentials: LoginRequest) {
    return this.http.post<LoginResponse>('api/authentication/login', credentials).pipe(
      tap(async (res: LoginResponse) => {
        await this.setTokens(res.token, res.refreshToken);
        // this.isAuthenticated.next(true);
      })
    );
  }

  async logout() {
    // this.isAuthenticated.next(false);
    await Preferences.remove({ key: TOKEN_KEY });
    await Preferences.remove({ key: REFRESH_TOKEN_KEY });
  }

  public refresh(){
    if (!this.token || !this.refreshToken) {
      throw new Error("Refresh not possible");
    }
    return this.http.post<LoginResponse>("api/authentication/refresh", { token: this.token, refreshToken: this.refreshToken })
    .pipe(
      tap( async res => {
        await this.setTokens(res.token, res.refreshToken);
      })
    );
  }

  private async setTokens(token: string, refreshToken: string) {
    this.token = token;
    this.refreshToken = refreshToken;
    await Preferences.set({ key: TOKEN_KEY, value: token });
    await Preferences.set({ key: REFRESH_TOKEN_KEY, value: refreshToken });
  }

  // public isUserAdmin = (): boolean => {
  //   const token = localStorage.getItem("token");
  //   if(!token) return false;
  //   const decodedToken = this.jwtHelper.decodeToken(token);
  //   const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
  //   return role === 'Administrator';
  // }

  // public getToken() {
  //   const token = localStorage.getItem("token");
  //   if(token && !this.jwtHelper.isTokenExpired(token)) return token;
  //   //TODO: Refresh Tokens werden für SignalR noch nicht richtig verwendet!
  //   throw new Error("Token is expired or not valid."); 
  // }

  // public forgotPassword(body: ForgotPasswordDto){
  //   return this.http.post('api/Accounts/ForgotPassword', body);
  // }

  // public resetPassword(body: ResetPasswordDto){
  //   return this.http.post('api/Accounts/ResetPassword', body);
  // }

  // public confirmEmail(token: string, email: string){
  //   let params = new HttpParams({ encoder: new CustomEncoder() })
  //   params = params.append('token', token);
  //   params = params.append('email', email);
  //   return this.http.get('api/Accounts/EmailConfirmation', params);
  // }

  // resendConfirmationEmail(email: string){
  //   let body = {
  //     "Email": email,
  //     "ClientURI": isAzure ? "https://notesapp1.azurewebsites.net/authentication/emailconfirmation" : 'https://localhost:44469/authentication/emailconfirmation'
  //   }
  //   return this.http.post('api/Accounts/SendMailVerification/', body);
  // }
}
