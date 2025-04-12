import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { AlertController } from "@ionic/angular";
import { catchError, from, switchMap, throwError } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ErrorHandelingInterceptor implements HttpInterceptor {
  
  constructor(
    private alertController: AlertController,
    private router: Router
    ) { }
  
  intercept(req: HttpRequest<any>, next: HttpHandler) {
    return next.handle(req)
    .pipe(
      catchError((error: HttpErrorResponse) => {
        if(error.status === 400 && error.error?.title && error.error.title === "E-Mail nicht verifiziert."){
          this.router.navigate(['auth', 'verify-email-info']);
          return this.createErrorObservable(error.error?.title, error.error?.detail);
        }
        if(error.status === 401) return throwError(() => error);
        return from(this.displayErrorMessage(error.error?.title, error.error?.detail)).pipe(
          switchMap(() => this.createErrorObservable(error.error?.title, error.error?.detail))
        );
      })
    )
  }

  private async displayErrorMessage(header: string | undefined, message: string | undefined){
    let alert = await this.alertController.create({
      header: header ?? "Unbekannter Fehler",
      message: message ?? "Ein unbekannter Fehler ist aufgetreten.",
      buttons: ['OK']
    });
    await alert.present();
  }

  private createErrorObservable(header: string | undefined, message: string | undefined){
    return throwError(() => new Error(`${header ?? "Unbekannter Fehler"}: ${message ?? "Ein unbekannter Fehler ist aufgetreten."}`));
  }

}
