import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';


@Injectable()
export class AccountService implements Resolve<any> 
{

  apiUrl: string;
  routeParams: any;
  account: any;
  onAccountChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _httpClient: HttpClient,
    @Inject('API_URL') private _apiUrl: string
  ) {
    this.apiUrl = `${_apiUrl}${ApiControllers.Accounts}/`;
   }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    this.routeParams = route.params;

    return new Promise((resolve, reject) => {
      Promise.all([
        this.getAccount()
      ]).then(() => {
        resolve();
      }, reject)
    });
  }

  getAccount(): Promise<any> {
    return new Promise((resolve, reject) => {

      if (this.routeParams.id === 'new') {
        this.onAccountChanged.next(false);
        resolve(false);
      }
      else 
      {
        this._httpClient.get(this.apiUrl + this.routeParams.id)
            .subscribe((response: any) => {
                this.account = response.payload;
                
                this.onAccountChanged.next(this.account);
                resolve(response);
            }, reject);
      }

    });
  }

  saveAccount(account) {
    return new Promise((resolve, reject) => {
      this._httpClient.put(this.apiUrl + account.id, account)
          .subscribe((response: any) => {
            resolve(response);
          }, reject);
    });
  }

  addAccount(account) {
    return new Promise((resolve, reject) => {
      this._httpClient.post(this.apiUrl, account)
          .subscribe((response: any) => {
            resolve(response);
          }, reject);
    });
  }

}
