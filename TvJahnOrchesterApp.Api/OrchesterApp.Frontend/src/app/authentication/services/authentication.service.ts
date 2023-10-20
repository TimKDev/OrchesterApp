import { Injectable } from '@angular/core';
import { Preferences } from '@capacitor/preferences';
import { BehaviorSubject, tap } from 'rxjs';
import { LoginRequest } from '../interfaces/login-request';
import { LoginResponse } from '../interfaces/login-response';
import { UnauthorizedHttpClientService } from 'src/app/core/services/unauthorized-http-client.service';
import { RegisterRequest } from '../interfaces/register-request';

export const TOKEN_KEY = 'token';
export const REFRESH_TOKEN_KEY = 'refresh-token';
export const CONNECTED_ORCHESTER_MITGLIED_KEY = 'connected-orchester-mitglieds-name';
export const USER_EMAIL_KEY = 'user-email';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  // isAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public token: string | null = null;
  public refreshToken: string | null = null;
  public connectedOrchesterMitgliedsName: string | null = null; 
  public userEmail: string | null = null; 

  constructor(
    private http: UnauthorizedHttpClientService,
  ) {
  }

  async loadTokensFromStorage() {
    const token = await Preferences.get({ key: TOKEN_KEY });
    const refreshToken = await Preferences.get({ key: REFRESH_TOKEN_KEY });
    const connectedOrchesterMitgliedsName = await Preferences.get({ key: CONNECTED_ORCHESTER_MITGLIED_KEY });
    const userEmail = await Preferences.get({ key: USER_EMAIL_KEY });
    this.token = token.value;
    this.refreshToken = refreshToken.value;
    this.connectedOrchesterMitgliedsName = connectedOrchesterMitgliedsName.value;
    this.userEmail = userEmail.value;
  }

  login(credentials: LoginRequest) {
    return this.http.post<LoginResponse>('api/authentication/login', credentials).pipe(
      tap(async (res: LoginResponse) => {
        await this.setTokens(res.token, res.refreshToken, res.name, res.email);
        // this.isAuthenticated.next(true);
      })
    );
  }

  register(registerRequest: RegisterRequest){
    return this.http.post<LoginResponse>('api/authentication/register', registerRequest).pipe(
      tap(async (res: LoginResponse) => {
        await this.setTokens(res.token, res.refreshToken, res.name, res.email);
      })
    );
  }

  async logout() {
    // this.isAuthenticated.next(false);
    await Preferences.remove({ key: TOKEN_KEY });
    await Preferences.remove({ key: REFRESH_TOKEN_KEY });
    await Preferences.remove({ key: CONNECTED_ORCHESTER_MITGLIED_KEY });
    await Preferences.remove({ key: USER_EMAIL_KEY });
  }

  public refresh(){
    if (!this.token || !this.refreshToken) {
      throw new Error("Refresh not possible");
    }
    return this.http.post<LoginResponse>("api/authentication/refresh", { token: this.token, refreshToken: this.refreshToken })
    .pipe(
      tap( async res => {
        await this.setTokens(res.token, res.refreshToken, res.name, res.email);
      })
    );
  }

  private async setTokens(token: string, refreshToken: string, name: string, userEmail: string) {
    this.token = token;
    this.refreshToken = refreshToken;
    this.connectedOrchesterMitgliedsName = name;
    this.userEmail = userEmail;
    await Preferences.set({ key: TOKEN_KEY, value: token });
    await Preferences.set({ key: REFRESH_TOKEN_KEY, value: refreshToken });
    await Preferences.set({ key: CONNECTED_ORCHESTER_MITGLIED_KEY, value: name });
    await Preferences.set({ key: USER_EMAIL_KEY, value: userEmail });
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
