
import { AuthService } from 'ngx-auth';
import { Inject, Injectable, OnDestroy } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpRequest } from '@angular/common/http';
import { from, Observable, of, Subject, Subscription, throwError } from 'rxjs';
import { TokenStorage } from './token-storage.service';
import { Router } from '@angular/router';
import { NgxPermissionsService } from 'ngx-permissions';
import { catchError, map, switchMap, tap } from 'rxjs/operators';

import { Credential } from './credential';
import { AccessData } from './access-data';
import { IPayload, Payload } from '../models/payload.model';
import { ApiPortalRoutes } from 'environments/app.constants';
import { ResetPasswordModel } from '../models/reset-password.model';

@Injectable()
export class AuthenticationService implements AuthService, OnDestroy {
	
	userData$: Subscription;
	userName$: Subscription;

    public onCredentialUpdated$: Subject<AccessData>;
    
    constructor(
		private http: HttpClient,
		private tokenStorage: TokenStorage,
		private permissionsService: NgxPermissionsService,
		@Inject('AUTH_URL') private _authUrl: string,
	) {
		this.onCredentialUpdated$ = new Subject();
    }

    ngOnDestroy(): void {
		this.userData$.unsubscribe();
		this.userName$.unsubscribe();
    }
    
   	/**
	 * Check, if user already authorized.
	 * @description Should return Observable with true or false values
	 * @returns {Observable<boolean>}
	 * @memberOf AuthService
	 */
	public isAuthorized(): Observable<boolean> {
		return this.tokenStorage.getAccessData().pipe(map((token: AccessData) => token && !!token.accessToken));
    }
    
  	/**
	 * Get access token
	 * @description Should return access token in Observable from e.g. localStorage
	 * @returns {Observable<string>}
	 */
	public getAccessToken(): Observable<string> {
		return this.tokenStorage.getAccessData().pipe(map((token: AccessData) => token !== null ? token.accessToken : ""));
    }

    	/**
	 * Function, that should perform refresh token verifyTokenRequest
	 * @description Should be successfully completed so interceptor
	 * can execute pending requests or retry original one
	 * @returns {Observable<any>}
	 */
	public refreshToken(): Observable<Payload<AccessData>> {
		return this.tokenStorage.getAccessData().pipe(
			switchMap((refreshToken: AccessData) => {
				return this.http.post<Payload<AccessData>>(this._authUrl + ApiPortalRoutes.validateToken, { xoken: refreshToken.refreshToken });
			}),
			tap(this.saveAccessData.bind(this)),
			catchError(err => {
				this.logout();
				return throwError(err);
			})
		);
    }
   
    /**
	 * Function, checks response of failed request to determine,
	 * whether token be refreshed or not.
	 * @description Essentialy checks status
	 * @param {Response} response
	 * @returns {boolean}
	 */
	public refreshShouldHappen(response: HttpErrorResponse): boolean {
		return response.status === 401;
    }
    
  	/**
	 * Verify that outgoing request is refresh-token,
	 * so interceptor won't intercept this request
	 * @param {string} url
	 * @returns {boolean}
	 */
	public verifyTokenRequest(url: string): boolean {
		return url.endsWith(ApiPortalRoutes.validateToken);
    }
    
    	/**
	 * Submit login request
	 * @param {Credential} credential
	 * @returns {Observable<any>}
	 */
	public login(credential: Credential): Observable<IPayload<AccessData>> {
		return this.http.post<any>(this._authUrl + ApiPortalRoutes.login, credential).pipe(
			map((result: any) => {
				if (result instanceof Array) {
					return result.pop();
				}
				this.tokenStorage.setUserName(credential.userName);
				return result;
			}),
			tap(this.saveAccessData.bind(this)),
			catchError(this.handleError('login', []))
		);
	}


	public forgotPassword(email) {
		return new Promise((resolve, reject) => {
			this.http.post<any>(this._authUrl + ApiPortalRoutes.forgotPassword, email)
			.subscribe((response: any) => {
				resolve(response);
			}, reject)
		});
		
	}

	public resetPassword(data) {
		return new Promise((resolve, reject) => {
			this.http.post<any>(this._authUrl + ApiPortalRoutes.resetPassword, data)
			.subscribe((response: any) => {
				resolve(response);
			}, reject)
		});
		
	}


    /**
	 * Handle Http operation that failed.
	 * Let the app continue.
	 * @param operation - name of the operation that failed
	 * @param result - optional value to return as the observable result
	 */
	private handleError<T>(operation = 'operation', result?: any) {
		return (error: any): Observable<any> => {
			// TODO: send the error to remote logging infrastructure
			console.error(error); // log to console instead

			// Let the app keep running by returning an empty result.
			return from(result);
		};
    }
    


    	/**
	 * Logout
	 */
	public logout(refresh?: boolean): void {
		this.tokenStorage.clear();
		if (refresh) {
			location.reload();
		}
    }
    
	/**
	 * Save access data in the storage
	 * @private
	 * @param {AccessData} data
	 */
	private saveAccessData(accessData: Payload<AccessData>) {
		if (!accessData.errors.message) {
			const perm = ['ADMIN'];
			this.permissionsService.loadPermissions(perm);
			accessData.payload.roles = perm;
			accessData.payload.timestamp = new Date().getTime();
			this.tokenStorage
				.setAccessData(accessData.payload)
				.setUserRoles(accessData.payload.roles);
			
			this.onCredentialUpdated$.next(accessData.payload);
		}
	}
	
	public getTokenTimeStamp(): number {
		let tStamp: number = 0;
		this.userData$ = this.tokenStorage.getAccessData().subscribe((userData: AccessData) => {
			tStamp = userData !== null ? userData.timestamp : 0;
		});
		return tStamp;
	}

	public getTokenExpirySeconds(): number {
		let tNumber: number = 0;
		this.userData$ = this.tokenStorage.getAccessData().subscribe((userData: AccessData) => {
			tNumber = userData !== null ? userData.accessTokenExpiresIn : 0;
		});
		return tNumber;
	}

	public getMemberUserName(): string {
		let name: string = '';
		this.userName$ = this.tokenStorage.getUserName().subscribe(returnStr => {
			name = returnStr;
		});
		return name;
	}
}