import { HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, catchError, filter, switchMap, throwError } from "rxjs";
import { AuthenticationService } from "src/app/authentication/services/authentication.service";
import { LoginResponse } from "src/app/authentication/interfaces/login-response";

@Injectable({
  providedIn: 'root'
})
export class RefreshTokenInterceptor {

  isRefreshing = false;
  refreshSubject = new BehaviorSubject<string | null>(null);

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
  ) { }

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(
      catchError(error => {
        if(error instanceof HttpErrorResponse && error.status === 401 && !req.url.includes("login")) {
          if(!this.isRefreshing){
            return this.tryToRefreshTokenAndResendRequest(req, next);
          }
          else{
            return this.awaitRefreshAndResendRequest(req, next);
          }
        }
        return throwError(() => error)
      })
    )
  }

  private awaitRefreshAndResendRequest(req: HttpRequest<unknown>, next: HttpHandler){
    return this.refreshSubject.pipe(
      filter(token => token !== null),
      switchMap(newToken => {
        let reqWithNewToken = req.clone({headers: req.headers.set("Authorization", `Bearer ${newToken}`)});
        return next.handle(reqWithNewToken);
      }),
      catchError((error) => {
        this.router.navigate(['auth']);
        return throwError(() => error.message);
      })
    )
  }

  private tryToRefreshTokenAndResendRequest(req: HttpRequest<unknown>, next: HttpHandler){
    this.isRefreshing = true;
    this.refreshSubject.next(null);
    return this.authenticationService.refresh().pipe(
      switchMap((res: LoginResponse) => {
        this.isRefreshing = false;
        this.refreshSubject.next(res.token);
        let reqWithNewToken = req.clone({headers: req.headers.set("Authorization", `Bearer ${res.token}`)});
        return next.handle(reqWithNewToken);
      }),
      catchError((error) => {
        this.isRefreshing = false;
        this.router.navigate(['auth']);
        return throwError(() => error.message);
      })
    );
  }
}
