import {Injectable} from "@angular/core";
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs";
import {SnackbarService} from "../Services/snackbar.service";

@Injectable()
export class JwtInterceptor implements HttpInterceptor{
  constructor(private _snackBarService : SnackbarService) {
  }
  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const potentialAccessToken = localStorage.getItem("token");
    if (potentialAccessToken == null){
      this._snackBarService.showErrorMessage("No access token provided")
    }
    else {
      const accessToken = JSON.parse(potentialAccessToken);
      req = req.clone({setHeaders: {Authorization: `Bearer ${accessToken}`}})
    }

    return next.handle(req);


  }

}
