import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RolesService {

  constructor() { }
}

export class RoleNames{
  public static readonly Admin = "Admin";
  public static readonly MusikalischerLeiter = "Musikalischer Leiter";
  public static readonly Vorstand = "Vorstand";
} 