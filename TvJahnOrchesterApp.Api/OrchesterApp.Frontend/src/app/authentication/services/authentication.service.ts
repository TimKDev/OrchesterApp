import { Injectable } from '@angular/core';
import { Preferences } from '@capacitor/preferences';
import { BehaviorSubject, tap } from 'rxjs';
import { LoginRequest } from '../interfaces/login-request';
import { LoginResponse } from '../interfaces/login-response';
import { BASE_PATH_FRONTEND, UnauthorizedHttpClientService } from 'src/app/core/services/unauthorized-http-client.service';
import { RegisterRequest } from '../interfaces/register-request';
import { ChangeEmailRequest } from '../interfaces/change-email-request';
import { ResendVerificationMailRequest } from '../interfaces/resend-verification-mail-request';
import { ConfirmEmailRequest } from '../interfaces/confirm-email-request';
import { ForgotPasswordRequest } from '../interfaces/forgot-password-request';
import { ResetPasswordRequest } from '../interfaces/reset-password-request';

export const TOKEN_KEY = 'token';
export const REFRESH_TOKEN_KEY = 'refresh-token';
export const CONNECTED_ORCHESTER_MITGLIED_KEY = 'connected-orchester-mitglieds-name';
export const USER_EMAIL_KEY = 'user-email';
export const USER_ROLES_KEY = 'user-roles';

export const CLIENT_URI_EMAIL_CONFIRMATION = `${BASE_PATH_FRONTEND}auth/email-confirmation`;
export const CLIENT_URI_PASSWORD_RESET = `${BASE_PATH_FRONTEND}auth/reset-password`;

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  public token: string | null = null;
  public refreshToken: string | null = null;
  public connectedOrchesterMitgliedsName: string | null = null; 
  public userEmail: string | null = null; 
  public userRoles: string[] | null = null;
  public userRoleSubject = new BehaviorSubject<string[]>([]); 

  constructor(
    private http: UnauthorizedHttpClientService,
  ) {
  }

  async loadTokensFromStorage() {
    const token = await Preferences.get({ key: TOKEN_KEY });
    const refreshToken = await Preferences.get({ key: REFRESH_TOKEN_KEY });
    const connectedOrchesterMitgliedsName = await Preferences.get({ key: CONNECTED_ORCHESTER_MITGLIED_KEY });
    const userEmail = await Preferences.get({ key: USER_EMAIL_KEY });
    const userRoles = await Preferences.get({key: USER_ROLES_KEY });
    this.token = token.value;
    this.refreshToken = refreshToken.value;
    this.connectedOrchesterMitgliedsName = connectedOrchesterMitgliedsName.value;
    this.userEmail = userEmail.value;
    this.userRoles = userRoles.value?.split(',') ?? [];
    this.userRoleSubject.next(this.userRoles);
  }

  login(credentials: LoginRequest) {
    this.userEmail = credentials.email;
    return this.http.post<LoginResponse>('api/authentication/login', credentials).pipe(
      tap(async (res: LoginResponse) => {
        await this.setTokens(res.token, res.refreshToken, res.name, res.email, res.userRoles);
      })
    );
  }

  register(registerRequest: RegisterRequest){
    return this.http.post<string>('api/authentication/register', registerRequest);
  }

  async logout() {
    await Preferences.remove({ key: TOKEN_KEY });
    await Preferences.remove({ key: REFRESH_TOKEN_KEY });
    await Preferences.remove({ key: CONNECTED_ORCHESTER_MITGLIED_KEY });
    await Preferences.remove({ key: USER_EMAIL_KEY });
    await Preferences.remove({ key: USER_ROLES_KEY });
  }

  public refresh(){
    if (!this.token || !this.refreshToken) {
      throw new Error("Refresh not possible");
    }
    return this.http.post<LoginResponse>("api/authentication/refresh", { token: this.token, refreshToken: this.refreshToken })
    .pipe(
      tap( async res => {
        await this.setTokens(res.token, res.refreshToken, res.name, res.email, res.userRoles);
      })
    );
  }

  public changeEmail(changeEmailRequest: ChangeEmailRequest){
    return this.http.put<void>('api/authentication/change-user-email', changeEmailRequest);
  }

  public resendVerificationMail(resendVerificationMailRequest: ResendVerificationMailRequest){
    return this.http.post<void>('api/authentication/resend-mail', resendVerificationMailRequest);
  }

  public confirmEmail(confirmEmailRequest: ConfirmEmailRequest){
    return this.http.post<void>('api/authentication/confirm-email', confirmEmailRequest);
  }

  public forgotPassword(request: ForgotPasswordRequest){
    return this.http.post<string>('api/authentication/forgot-password', request);
  }

  public resetPassword(request: ResetPasswordRequest){
    return this.http.post<string>('api/authentication/resetPassword', request);
  }

  private async setTokens(token: string, refreshToken: string, name: string, userEmail: string, userRoles: string[]) {
    this.token = token;
    this.refreshToken = refreshToken;
    this.connectedOrchesterMitgliedsName = name;
    this.userEmail = userEmail;
    this.userRoles = userRoles;
    this.userRoleSubject.next(userRoles);
    await Preferences.set({ key: TOKEN_KEY, value: token });
    await Preferences.set({ key: REFRESH_TOKEN_KEY, value: refreshToken });
    await Preferences.set({ key: CONNECTED_ORCHESTER_MITGLIED_KEY, value: name });
    await Preferences.set({ key: USER_EMAIL_KEY, value: userEmail });
    await Preferences.set({key: USER_ROLES_KEY, value: userRoles.toString()})
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
