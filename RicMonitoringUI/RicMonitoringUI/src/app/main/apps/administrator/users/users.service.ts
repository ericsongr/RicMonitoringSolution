import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiControllers } from 'environments/api-controllers';
import { HttpClient } from '@angular/common/http';
import { ApiPortalRoutes } from 'environments/app.constants';

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

    var url = `${this.authUrl}/${ApiControllers.UserProfile}`;

    return new Promise((resolve, reject) => {
      
      this._httpClient.get(url)
          .subscribe((response: any) => {
             
            this.users = response.payload;
             
              this.onUsersChanged.next(this.users);
            
              resolve(response);
          
            }, reject);
    })

    
  }

}
