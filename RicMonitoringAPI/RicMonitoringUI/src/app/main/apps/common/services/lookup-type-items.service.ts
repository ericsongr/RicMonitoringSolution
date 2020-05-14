import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable()
export class LookupTypeItemsService implements Resolve<any> {

  onLookupTypeItemsChanged: BehaviorSubject<any> = new BehaviorSubject({});
  constructor(private _httpClient: HttpClient,
             @Inject('API_URL') private _apiUrl: string) { }

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
      var url = `${this._apiUrl}${ApiControllers.LookupTypeItems}/${parameterName}?fields=id,description`;
      this._httpClient.get(url)
          .subscribe((lookupTypeItems: any) => {
              this.onLookupTypeItemsChanged.next(lookupTypeItems);
              resolve(lookupTypeItems);
          }, reject);
    })
  }

}



