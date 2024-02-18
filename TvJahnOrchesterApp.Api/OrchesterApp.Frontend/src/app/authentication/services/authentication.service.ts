import { Injectable } from '@angular/core';
import { Preferences } from '@capacitor/preferences';
import { BehaviorSubject, tap } from 'rxjs';
import { LoginRequest } from '../interfaces/login-request';
import { LoginResponse } from '../interfaces/login-response';
import { UnauthorizedHttpClientService } from 'src/app/core/services/unauthorized-http-client.service';
import { RegisterRequest } from '../interfaces/register-request';
import { ChangeEmailRequest } from '../interfaces/change-email-request';
import { ResendVerificationMailRequest } from '../interfaces/resend-verification-mail-request';
import { ConfirmEmailRequest } from '../interfaces/confirm-email-request';
import { ForgotPasswordRequest } from '../interfaces/forgot-password-request';
import { ResetPasswordRequest } from '../interfaces/reset-password-request';
import { environment } from 'src/environments/environment';

export const TOKEN_KEY = 'token';
export const REFRESH_TOKEN_KEY = 'refresh-token';
export const CONNECTED_ORCHESTER_MITGLIED_KEY = 'connected-orchester-mitglieds-name';
export const CONNECTED_ORCHESTER_MITGLIED_IMAGE_KEY = 'connected-orchester-mitglieds-image';
export const USER_EMAIL_KEY = 'user-email';
export const USER_ROLES_KEY = 'user-roles';

export const CLIENT_URI_EMAIL_CONFIRMATION = `${environment.basePathFrontend}auth/email-confirmation`;
export const CLIENT_URI_PASSWORD_RESET = `${environment.basePathFrontend}auth/reset-password`;
export const CLIENT_URI_REGISTRATION = `${environment.basePathFrontend}auth/registration`;

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  public token: string | null = null;
  public refreshToken: string | null = null;
  public connectedOrchesterMitgliedsName: string | null = null; 
  public userEmail: string | null = null; 
  public userRoles: string[] | null = null;
  public userImage: string | null = null;
  public userRoleSubject = new BehaviorSubject<string[]>([]); 

  constructor(
    private http: UnauthorizedHttpClientService,
  ) {
  }

  async loadTokensFromStorage() {
    const token = await Preferences.get({ key: TOKEN_KEY });
    const refreshToken = await Preferences.get({ key: REFRESH_TOKEN_KEY });
    const connectedOrchesterMitgliedsName = await Preferences.get({ key: CONNECTED_ORCHESTER_MITGLIED_KEY });
    const connectedOrchesterMitgliedsImage = await Preferences.get({ key: CONNECTED_ORCHESTER_MITGLIED_IMAGE_KEY });
    const userEmail = await Preferences.get({ key: USER_EMAIL_KEY });
    const userRoles = await Preferences.get({key: USER_ROLES_KEY });
    this.token = token.value;
    this.refreshToken = refreshToken.value;
    this.connectedOrchesterMitgliedsName = connectedOrchesterMitgliedsName.value;
    this.userImage = connectedOrchesterMitgliedsImage.value;
    this.userEmail = userEmail.value;
    this.userRoles = userRoles.value?.split(',') ?? [];
    this.userRoleSubject.next(this.userRoles);
  }

  login(credentials: LoginRequest) {
    this.userEmail = credentials.email;
    return this.http.post<LoginResponse>('api/authentication/login', credentials).pipe(
      tap(async (res: LoginResponse) => {
        await this.setTokens(res.token, res.refreshToken, res.name, res.email, res.userRoles, res.image);
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
    await Preferences.remove({ key: CONNECTED_ORCHESTER_MITGLIED_IMAGE_KEY });
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
        await this.setTokens(res.token, res.refreshToken, res.name, res.email, res.userRoles, res.image);
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

  private async setTokens(token: string, refreshToken: string, name: string, userEmail: string, userRoles: string[], image?: string) {
    this.token = token;
    this.refreshToken = refreshToken;
    this.connectedOrchesterMitgliedsName = name;;
    this.userEmail = userEmail;
    this.userRoles = userRoles;
    this.userRoleSubject.next(userRoles);
    await Preferences.set({ key: TOKEN_KEY, value: token });
    await Preferences.set({ key: REFRESH_TOKEN_KEY, value: refreshToken });
    await Preferences.set({ key: CONNECTED_ORCHESTER_MITGLIED_KEY, value: name });
    if(image){
      this.userImage = image;
      await Preferences.set({ key: CONNECTED_ORCHESTER_MITGLIED_IMAGE_KEY, value: image });
    }
    await Preferences.set({ key: USER_EMAIL_KEY, value: userEmail });
    await Preferences.set({key: USER_ROLES_KEY, value: userRoles.toString()})
  }
}
