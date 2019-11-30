import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';

const API_URL = environment.webApi + ApiControllers.Renters;
const TABLE_FIELDS = "?fields=id,name,advancePaidDateString,startDateString,dueDateString,noOfPersons&orderBy=name";
const DROPDOWN_FIELDS = "?fields=id,name";

@Injectable()
export class RentersService implements Resolve<any> {

  renters: any[];
  onRentersChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(private _httpClient: HttpClient)  
  { 
    
  }
  
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getRenters("table")
      ]).then(() => {
        resolve();
      }, reject);
    });
  }

  getRenters(htmlComponent): Promise<any> {
    var fields = TABLE_FIELDS;
    if( htmlComponent == "dropdown"){
      fields = DROPDOWN_FIELDS;
    }

    return new Promise((resolve, reject) => {
      var url = `${API_URL}${fields}`
      this._httpClient.get(url)
          .subscribe((response: any) => {
              this.renters = response;
              this.onRentersChanged.next(this.renters);
              resolve(response);
          }, reject);
    })
  }


}
