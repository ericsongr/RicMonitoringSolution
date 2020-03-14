import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

const API_URL = `${environment.webApi}${ApiControllers.LookupTypeItems}`;

@Injectable()
export class LookupTypeItemsService implements Resolve<any> {

  onLookupTypeItemsChanged: BehaviorSubject<any> = new BehaviorSubject({});
  constructor(private _httpClient: HttpClient) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getItemsByLookupType("ages")
      ]).then(() => {
        resolve();
      }, reject);
    });
  }

  getItemsByLookupType(parameterName): Promise<any> {

    return new Promise((resolve, reject) => {
      var url = `${API_URL}/${parameterName}?fields=id,description`;
      this._httpClient.get(url)
          .subscribe((lookupTypeItems: any) => {
              this.onLookupTypeItemsChanged.next(lookupTypeItems);
              resolve(lookupTypeItems);
          }, reject);
    })
  }

}



