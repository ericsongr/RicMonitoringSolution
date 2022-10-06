import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { HttpClient } from '@angular/common/http';
import { UserPushNotification } from './user-push-notification.model';


@Injectable()
export class UserProfileService implements Resolve<any> 
{

  userUrl: string;
  apiPushNotification: string;
  roleUrl: string;
  routeParams: any;
  user: any;
  
  onUserChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _http : HttpClient, 
    @Inject('AUTH_URL') private _authUrl: string,
    @Inject('API_URL') private _API_URL: string,
  ) {
    this.userUrl = `${_authUrl}/${ApiControllers.UserProfile}`;
    this.roleUrl = `${_authUrl}/${ApiControllers.Role}`;

    this.apiPushNotification = `${_API_URL}${ApiControllers.PushNotification}`;
   }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    this.routeParams = route.params;

    return new Promise((resolve, reject) => {
      Promise.all([
        this.getUser()
      ]).then(() => {
        resolve();
      }, reject)
    });
  }

  getRoles() : Promise<any> {

    return new Promise((resolve, reject) => {
      this._http.get(this.roleUrl)
        .subscribe((response: any) => {
           resolve(response.payload);
        }, reject)
    })

  }
  
  getUser(): Promise<any> {
    return new Promise((resolve, reject) => {

      if (this.routeParams.id === 'new') {
        this.onUserChanged.next(false);
        resolve(false);
      }
      else 
      {
        var url = `${this.userUrl}/profile?id=${this.routeParams.id}`;
        
        this._http.get(url)
            .subscribe((response: any) => {

                this.user = response;
               
                this.onUserChanged.next(this.user);

                resolve(response);

            }, reject);
      }

    });
  }

  saveUser(user){
    return new Promise((resolve, reject) => {
      this._http.put(this.userUrl + '/update-profile', user)
          .subscribe((response: any) => {
            resolve(response);
          }, reject);
    });
  }

  pushNotification(userPushNotification: UserPushNotification) {

    return new Promise((resolve, reject) => {
      debugger;
      this._http.post(this.apiPushNotification, userPushNotification)
          .subscribe((response: any) => {
            resolve(response);
          }, reject);
    });
  }

}
