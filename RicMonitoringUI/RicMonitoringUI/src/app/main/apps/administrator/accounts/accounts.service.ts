import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ApiControllers } from 'environments/api-controllers';
const TABLE_FIELDS = "?fields=id,name,isActive&orderBy=name";
const DROPDOWN_FIELDS = "?fields=id,name&orderBy=name";

@Injectable()
export class AccountsService implements Resolve<any> {

  accounts: any[];
  onAccountsChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _httpClient: HttpClient,
    @Inject('API_URL') private apiUrl: string)  
  { 
    
  }
  
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getAccounts("table")
      ]).then(() => {
        resolve();
      }, reject);
    });
  }

  getAccounts(htmlComponent, id = 0): Promise<any> {
    var fields = TABLE_FIELDS;
    var url = `${this.apiUrl}${ApiControllers.Accounts}${fields}`;
    
    if (htmlComponent == "dropdown"){
      fields = DROPDOWN_FIELDS.replace('?', '');
      url = `${this.apiUrl}${ApiControllers.Accounts}?id=${id}&${fields}`;
    }

    return new Promise((resolve, reject) => {
      
      this._httpClient.get(url)
          .subscribe((response: any) => {
              this.accounts = response.payload;
              this.onAccountsChanged.next(this.accounts);
              resolve(response);
          }, reject);
    })

    
  }

  setAccountId(accountId: string) {
    localStorage.setItem('AccountId', accountId)
  }

  getSelectedAccountId() : string {
    var accountId = localStorage.getItem('AccountId');
    return accountId;
  }


}
