import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  public numberOfUnread = new BehaviorSubject<number>(0);

  constructor() { }

}
