import { Injectable } from '@angular/core';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class RolesService {

  public isCurrentUserAdmin = false;
  public isCurrentUserVorstand = false;
  public isCurrentUserMusikalischerLeiter = false;

  constructor(
    private authService: AuthenticationService
  ) {
    this.authService.userRoleSubject.subscribe(userRoles => {
      this.isCurrentUserAdmin = userRoles.includes(RoleNames.Admin) ?? false;
      this.isCurrentUserVorstand = userRoles.includes(RoleNames.Vorstand) ?? false;
      this.isCurrentUserMusikalischerLeiter = userRoles.includes(RoleNames.MusikalischerLeiter) ?? false;
    });
  }

}

export class RoleNames {
  public static readonly Admin = "Admin";
  public static readonly MusikalischerLeiter = "Musikalischer Leiter";
  public static readonly Vorstand = "Vorstand";
} 