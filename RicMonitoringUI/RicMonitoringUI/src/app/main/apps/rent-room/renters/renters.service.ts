import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiControllers } from 'environments/api-controllers';
import { HttpClient } from '@angular/common/http';
import { AccountsService } from '../../administrator/accounts/accounts.service';

const TABLE_FIELDS = "&fields=id,name,advancePaidDateString,startDateString,dueDayString,dueDay,noOfPersons&orderBy=dueDay";
const DROPDOWN_FIELDS = "?fields=id,name";

@Injectable()
export class RentersService implements Resolve<any> {

  renters: any[];
  onRentersChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _accountsService: AccountsService,
    private _http: HttpClient,
    @Inject('API_URL') private _apiUrl: string) {}
  
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
      var accountId = this._accountsService.getSelectedAccountId();
      var url = `${this._apiUrl}${ApiControllers.Renters}?accountId=${accountId}${fields}`
      this._http.get(url)
          .subscribe((response: any) => {
              this.renters = response.payload;
              this.onRentersChanged.next(this.renters);
              resolve(response);
          }, reject);
    })
  }


}
