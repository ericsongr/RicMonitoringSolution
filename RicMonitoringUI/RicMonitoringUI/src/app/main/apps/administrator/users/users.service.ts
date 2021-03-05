import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiControllers } from 'environments/api-controllers';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiPortalRoutes } from 'environments/app.constants';
// import { OidcSecurityService } from 'angular-auth-oidc-client';

@Injectable()
export class UsersService implements Resolve<any> {

  users: any[];
  onUsersChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _httpClient: HttpClient,
    @Inject('AUTH_URL') private authUrl: string)  
  { 
    
  }
  
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getUsers()
      ]).then(() => {
        resolve();
      }, reject);
    });
  }

  changePassword(formData) {

    var url = `${this.authUrl}${ApiPortalRoutes.changePassword}`;
    return new Promise((resolve, reject) => {
      this._httpClient.post(url, formData)
          .subscribe((response: any) => {
            resolve(response);
          }, reject);
    });

  }

  getUsers(): Promise<any> {

    var url = `${this.authUrl}/api/${ApiControllers.Account}`;

    return new Promise((resolve, reject) => {
      
      var httpHeaders = this.getHeaders();
      
      this._httpClient.get(url, {headers: httpHeaders} )
          .subscribe((response: any) => {
             
            this.users = response.payload;
             
              this.onUsersChanged.next(this.users);
            
              resolve(response);
          
            }, reject);
    })

    
  }

  public getHeaders() {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json');
    headers = headers.set('Accept', 'application/json');
    return this.appendAuthHeader(headers);
  }

  public getToken() {
      const token = ''; // this._oidcSecurityService.getToken();
      return token;
  }

  private appendAuthHeader(headers: HttpHeaders) {
      // const token = this._oidcSecurityService.getToken();
      const token = '';
      if (token === '') { return headers; }
      const tokenValue = 'Bearer ' + token;
      
      return headers.set('Authorization', tokenValue);
  }

}
