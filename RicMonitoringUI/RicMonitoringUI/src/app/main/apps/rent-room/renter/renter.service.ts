import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { AuthService } from '../../common/core/auth/auth.service';

@Injectable()
export class RenterService implements Resolve<any> 
{
  apiUrl: string;
  routeParams: any;
  renter: any;
  onRenterChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _authService : AuthService,
    @Inject('API_URL') private _apiUrl: string) 
  { 
    this.apiUrl = `${_apiUrl}${ApiControllers.Renters}/`;
  }

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
        this._authService.get(this.apiUrl + this.routeParams.id)
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
      this._authService.put(this.apiUrl + renter.id, renter)
          .subscribe((response: any) => {
            resolve(response.id);
          }, reject);
    });
  }

  addRenter(renter){
    return new Promise((resolve, reject) => {
      this._authService.post(this.apiUrl, renter)
          .subscribe((response: any) => {
            resolve(response.id);
          }, reject);
    });
  }

}
