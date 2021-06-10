import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { ApiControllers } from 'environments/api-controllers';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable()
export class SettingsService implements Resolve<any> {

  settings: any[];
  onSettingsChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _httpClient: HttpClient,
    @Inject('API_URL') private apiUrl: string
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getSettings()
      ]).then(() => {
        resolve('success');
      }, reject);
    });
  }

  getSettings() : Promise<any> {
    
    var url = `${this.apiUrl}${ApiControllers.Settings}?fields=id,key,realValue,friendlyName,dataType`;

    return new Promise((resolve, reject) => {

      this._httpClient.get(url)

          .subscribe((response: any) => {

            this.settings = response.payload;

            this.onSettingsChanged.next(this.settings);

            resolve(this.settings);

          }, reject)

    })
  }

  save(setting){
    return new Promise((resolve, reject) => {
      var url = `${this.apiUrl}${ApiControllers.Settings}/0` //set 0 the id just to have value
      this._httpClient.put(url, setting)
          .subscribe((response: any) => {
            resolve(response);
          }, reject);

    });
  }

}
