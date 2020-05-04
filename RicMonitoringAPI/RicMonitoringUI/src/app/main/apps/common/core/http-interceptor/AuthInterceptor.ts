import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { AuthService } from "../auth/auth.service";
import { Observable, throwError } from "rxjs";
import { Injectable } from "@angular/core";
import { catchError } from "rxjs/operators";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(
        private _authService: AuthService
    ) { }

    intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        //Get the auth token from the service
        
      

        // this._authService.getIsAuthorized().subscribe((isAuthorized: boolean) => {
            
            // if (isAuthorized) {

            //         var authToken = this._authService.getToken();
                
            //         //clone the request and replace the original headers with
            //         //cloned headers, updated with the authorization
            //         const authReq = httpRequest.clone({
            //           setHeaders: {
            //               'Authorization': 'bearer ' + authToken,
            //               'Content-Type' : 'application/json',
            //               'Accept'       : 'application/json'
            //           }  
            //         })
            //         debugger;
            //         // send cloned request with header to handler
            //         return next.handle(authReq).pipe(
            //             catchError(err => {
            //                 console.log('handle error: ', err);
                            
            //                 const error = err.error.message || err.statusText;
                            
            //                 this._authService.logout();

            //                 return throwError(error);
            //             })
            //         )
        
            //     }

            // });

            return next.handle(httpRequest);
       
    }
}
