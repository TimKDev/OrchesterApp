import { Injectable, OnDestroy } from "@angular/core";
import { Observable, Subject, takeUntil } from "rxjs";

@Injectable()
export class Unsubscribe implements OnDestroy{
  protected unsubscribe$ = new Subject<void>();

  ngOnDestroy(): void {
    this.unsubscribe();
  }

  public unsubscribe(){
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public autoUnsubscribe<T>(obs: Observable<T>){
    return obs.pipe(takeUntil(this.unsubscribe$))
  }
}