import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { AuthService } from '../../common/core/auth/auth.service';

const API_URL = environment.webApi + ApiControllers.Renters + "/";

@Injectable()
export class RenterService implements Resolve<any> 
{

  routeParams: any;
  renter: any;
  onRenterChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _authService : AuthService
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    this.routeParams = route.params;

    return new Promise((resolve, reject) => {
      Promise.all([
        this.getRenter()
      ]).then(() => {
        resolve();
      }, reject)
    });
  }

  getRenter(): Promise<any> {
    return new Promise((resolve, reject) => {

      if (this.routeParams.id === 'new') {
        this.onRenterChanged.next(false);
        resolve(false);
      }
      else 
      {
        this._authService.get(API_URL + this.routeParams.id)
            .subscribe((response: any) => {
                this.renter = response;
                this.onRenterChanged.next(this.renter);
                resolve(response);
            }, reject);
      }

    });
  }

  saveRenter(renter){
    return new Promise((resolve, reject) => {
      this._authService.put(API_URL + renter.id, renter)
          .subscribe((response: any) => {
            resolve(response.id);
          }, reject);
    });
  }

  addRenter(renter){
    return new Promise((resolve, reject) => {
      this._authService.post(API_URL, renter)
          .subscribe((response: any) => {
            resolve(response.id);
          }, reject);
    });
  }

}
