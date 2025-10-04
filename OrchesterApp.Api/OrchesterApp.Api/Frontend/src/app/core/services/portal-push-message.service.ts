import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject, firstValueFrom } from 'rxjs';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';
import { environment } from 'src/environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class PortalPushMessageService {
  private hubConnection?: HubConnection;
  private jwtHelper = new JwtHelperService();

  public portalPushMessageSubject = new Subject<PortalPushMessage>();

  constructor(
    private authService: AuthenticationService
  ) { }

  /**
   * Check if the JWT token is expired or about to expire (within 30 seconds)
   */
  private isTokenExpired(token: string | null): boolean {
    if (!token) return true;
    
    try {
      // Check if token is expired or will expire in the next 30 seconds
      const expirationDate = this.jwtHelper.getTokenExpirationDate(token);
      if (!expirationDate) return true;
      
      const bufferTime = 30 * 1000; // 30 seconds buffer
      return Date.now() >= (expirationDate.getTime() - bufferTime);
    } catch (error) {
      console.error('Error checking token expiration:', error);
      return true;
    }
  }

  /**
   * Get a valid access token, refreshing if necessary
   */
  private async getValidToken(): Promise<string> {
    if (!this.authService.token) {
      await this.authService.loadTokensFromStorage();
    }

    if (this.isTokenExpired(this.authService.token)) {
      console.log('SignalR: Token expired, attempting to refresh...');
      try {
        await firstValueFrom(this.authService.refresh());
      } catch (error) {
        console.error('SignalR: Failed to refresh token', error);
      }
    }

    return this.authService.token || '';
  }

  public async startConnection(): Promise<void> {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.basePathBackend}hubs/portal-push-message`, {
        accessTokenFactory: async () => {
          return await this.getValidToken();
        }
      })
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: retryContext => {
          // If we get a 401 (unauthorized), try to refresh the token
          // before attempting to reconnect
          if (retryContext.elapsedMilliseconds < 60000) {
            // Retry after 2, 5, 10 seconds, etc.
            return Math.min(1000 * Math.pow(2, retryContext.previousRetryCount), 30000);
          } else {
            // Stop reconnecting after 1 minute
            console.log('SignalR: Stopping reconnection attempts');
            return null;
          }
        }
      })
      .build();

    await this.hubConnection
      .start()
      .then(() => console.log('SignalR Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));

    this.hubConnection?.on('Receive', (message: PortalPushMessage) => {
      this.portalPushMessageSubject.next(message);
    });
  }

  public stopConnection(): Promise<void> | undefined {
    return this.hubConnection?.stop();
  }
}

export interface PortalPushMessage {
  type: string,
  data: any
}

export enum PortalPushMessageTypes {
  Notifications = "Notifications",
  Termins = "Termins",
  MitgliedOnlineStatus = "MitgliedOnlineStatus"
}
