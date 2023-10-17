import { HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, catchError, switchMap, throwError } from "rxjs";
import { AuthenticationService } from "src/app/authentication/services/authentication.service";
import { LoginResponse } from "src/app/authentication/interfaces/login-response";

@Injectable({
  providedIn: 'root'
})
export class RefreshTokenInterceptor {

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
  ) { }

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(
      catchError(error => {
        debugger;
        if(error instanceof HttpErrorResponse && error.status === 401 && !req.url.includes("login")) {
          return this.tryToRefreshTokenAndResendRequest(req, next);
        }
        return throwError(() => error)
      })
    )
  }

  private tryToRefreshTokenAndResendRequest(req: HttpRequest<unknown>, next: HttpHandler){
    return this.authenticationService.refresh().pipe(
      switchMap((res: LoginResponse) => {
        debugger;
        let reqWithNewToken = req.clone({headers: req.headers.set("Authorization", `Bearer ${res.token}`)});
        return next.handle(reqWithNewToken);
      }),
      catchError((error) => {
        this.router.navigate(['auth']);
        return throwError(() => error.message);
      })
    );
  }
}
