import { Injectable, OnDestroy, Inject } from '@angular/core';
import { OidcSecurityService, OpenIdConfiguration, AuthWellKnownEndpoints, AuthorizationResult, AuthorizationState } from 'angular-auth-oidc-client';
import { Observable ,  Subscription, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { UserDataService } from 'app/main/apps/administrator/users/user-data.service';
import { FuseNavigationService } from '@fuse/components/navigation/navigation.service';

@Injectable()
export class AuthService implements OnDestroy {

    isAuthorized = false;
    navigation: any;
    userDataService: UserDataService;

    constructor(
        private _oidcSecurityService: OidcSecurityService,
        private _http: HttpClient,
        private _router: Router,
        private _fuseNavigationService: FuseNavigationService,
        @Inject('BASE_URL') private originUrl: string,
        @Inject('AUTH_URL') private authUrl: string,
    ) {
    }

    public username: string;
    
    private isAuthorizedSubscription: Subscription = new Subscription;
    
    ngOnDestroy(): void {
        if (this.isAuthorizedSubscription) {
            this.isAuthorizedSubscription.unsubscribe();
        }
    }

    public initAuth() {
        const openIdConfiguration: OpenIdConfiguration = {
            stsServer: this.authUrl,
            redirect_url: this.originUrl + 'callback',
            client_id: 'spaRicMonitoringCodeClient',
            response_type: 'code',
            scope: 'openid profile RicMonitoringAPI',
            post_logout_redirect_uri: this.originUrl,
            forbidden_route: '/forbidden',
            unauthorized_route: '/unauthorized',
            silent_renew: true,
            silent_renew_url: this.originUrl + 'silent-renew.html',
            history_cleanup_off: true,
            auto_userinfo: true,
            log_console_warning_active: false,
            log_console_debug_active: false,
            max_id_token_iat_offset_allowed_in_seconds: 10,
        };
        
        const authWellKnownEndpoints: AuthWellKnownEndpoints = {
            issuer: this.authUrl,
            jwks_uri: this.authUrl + '/.well-known/openid-configuration/jwks',
            authorization_endpoint: this.authUrl + '/connect/authorize',
            token_endpoint: this.authUrl + '/connect/token',
            userinfo_endpoint: this.authUrl + '/connect/userinfo',
            end_session_endpoint: this.authUrl + '/connect/endsession',
            check_session_iframe: this.authUrl + '/connect/checksession',
            revocation_endpoint: this.authUrl + '/connect/revocation',
            introspection_endpoint: this.authUrl + '/connect/introspect',
        };
       
        this._oidcSecurityService.setupModule(openIdConfiguration, authWellKnownEndpoints);

        if (this._oidcSecurityService.moduleSetup) {
            this.doCallbackLogicIfRequired();
        } else {
            this._oidcSecurityService.onModuleSetup.subscribe(() => {
                this.doCallbackLogicIfRequired();
            });
        }
        this.isAuthorizedSubscription = this._oidcSecurityService.getIsAuthorized().subscribe((isAuthorized => {
            this.isAuthorized = isAuthorized;


        }));

        this._oidcSecurityService.onAuthorizationResult.subscribe(
            (authorizationResult: AuthorizationResult) => {
                this.onAuthorizationResultComplete(authorizationResult);

                localStorage.setItem('isCallback', 'true'); 

            });

    }

    private onAuthorizationResultComplete(authorizationResult: AuthorizationResult) {
        
        // console.log('Auth result received AuthorizationState:'
        //     + authorizationResult.authorizationState
        //     + ' validationResult:' + authorizationResult.validationResult);

        if (authorizationResult.authorizationState === AuthorizationState.unauthorized) {
            if (window.parent) {
                // sent from the child iframe, for example the silent renew
                this._router.navigate(['/unauthorized']);
            } else {
                window.location.href = '/unauthorized';
            }
        }
    }

    private doCallbackLogicIfRequired() {
        this._oidcSecurityService.authorizedCallbackWithCode(window.location.toString());
    }

    getIsAuthorized(): Observable<boolean> {
        return this._oidcSecurityService.getIsAuthorized();
    }

    getUserData() : Observable<any> {
        return this._oidcSecurityService.getUserData()
    }

    login() {
        localStorage.setItem('isCallback', 'false');
        this._oidcSecurityService.authorize();
    }

    logout() {
        
        this._oidcSecurityService.logoff();
    
    }

    get(url: string): Observable<any> {
        return this._http.get(url, { headers: this.getHeaders() })
        .pipe(catchError((error) => {
            this._oidcSecurityService.handleError(error);
            return throwError(error);
        }));
    }

    put(url: string, data: any): Observable<any> {
        const body = JSON.stringify(data);
        return this._http.put(url, body, { headers: this.getHeaders() })
        .pipe(catchError((error) => {
            this._oidcSecurityService.handleError(error);
            return throwError(error);
        }));
    }

    delete(url: string): Observable<any> {
        return this._http.delete(url, { headers: this.getHeaders() })
        .pipe(catchError((error) => {
            this._oidcSecurityService.handleError(error);
            return throwError(error);
        }));
    }

    post(url: string, body: any): Observable<any> {
        return this._http.post(url, body, { headers: this.getHeaders() })
        .pipe(catchError((error) => {
            this._oidcSecurityService.handleError(error);
            return throwError(error);
        }));
    }

    public getHeaders() {
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Accept', 'application/json');
        return this.appendAuthHeader(headers);
    }

    public getToken() {
        const token = this._oidcSecurityService.getToken();
        return token;
    }

    private appendAuthHeader(headers: HttpHeaders) {
        const token = this._oidcSecurityService.getToken();
        
        if (token === '') { return headers; }
        const tokenValue = 'Bearer ' + token;
        return headers.set('Authorization', tokenValue);
    }
}
