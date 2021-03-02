import { Injectable } from '@angular/core';
import { AuthenticationService } from '../auth/authentication.service';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor,
    HttpResponse,
    HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError, from } from 'rxjs';
import { Router } from '@angular/router';
import { catchError, tap, map } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private auth: AuthenticationService,
        private router: Router, ) { }

    intercept(req: HttpRequest<any>, next: HttpHandler):
        Observable<HttpEvent<any>> {

        // Get the auth token from the service.
        let authToken = "";
        this.auth.getAccessToken().subscribe(token => {
            authToken = token;
        });

        // Clone the request and replace the original headers with
        // cloned headers, updated with the authorization.
        let authReq = req.clone({
            setHeaders: {
                'Authorization': 'bearer ' + authToken,
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        //change header when upload image
        if (req.url.indexOf("renter-file-upload/web") > 0) {
            authReq = req.clone({
                setHeaders: {
                    'Authorization': 'bearer ' + authToken,
                    'Content-Type': 'multipart/form-data',
                }
            });
        }
        
        // send cloned request with header to the next handler.
        return next.handle(authReq).pipe(
            catchError(err => {
                 return throwError(err);
            }))
    }

    // private isTokenExpired(): boolean {
    //     const token = this.auth.getTokenTimeStamp();
    //     const expiry_in_seconds = this.auth.getTokenExpirySeconds();
    //     if (token > 0) {
    //         const now = new Date().getTime();
    //         const timestamp = (token + (expiry_in_seconds * 1000));
    //         return now > timestamp;            
    //     }
    //     return true;
    // }

    // private handleError(error: HttpErrorResponse) {
    //     if (error.error instanceof ErrorEvent) {
    //         // A client-side or network error occurred. Handle it accordingly.
    //         console.error('An error occurred:', error.error.message);
    //     } else {
    //         // The backend returned an unsuccessful response code.
    //         // The response body may contain clues as to what went wrong,
    //         console.error(
    //             `Backend returned code ${error.status}, ` +
    //             `body was: ${error.error}`);
    //     }
    //     // return an observable with a user-facing error message
    //     return throwError(
    //         'Something bad happened; please try again later.');
    // };
}