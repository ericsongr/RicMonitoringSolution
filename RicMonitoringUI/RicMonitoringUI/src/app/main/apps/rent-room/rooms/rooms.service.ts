import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ApiControllers } from 'environments/api-controllers';
import { AccountsService } from '../../administrator/accounts/accounts.service';
const TABLE_FIELDS = "&fields=id,name,frequency,price&orderBy=name";
const DROPDOWN_FIELDS = "?fields=id,name,isOccupied,price&orderBy=name";

@Injectable()
export class RoomsService implements Resolve<any> {

  rooms: any[];
  onRoomsChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _accountsService: AccountsService,
    private _httpClient: HttpClient,
    @Inject('API_URL') private apiUrl: string)  
  { 
    
  }
  
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getRooms("table")
      ]).then(() => {
        resolve();
      }, reject);
    });
  }

  getRooms(htmlComponent, renterId = 0): Promise<any> {

    var fields = TABLE_FIELDS;
    var accountId = this._accountsService.getSelectedAccountId();
    var url = `${this.apiUrl}${ApiControllers.Rooms}?accountId=${accountId}${fields}`;
    
    if (htmlComponent == "dropdown"){
      fields = DROPDOWN_FIELDS.replace('?', '');
      url = `${this.apiUrl}${ApiControllers.Rooms}?renterId=${renterId}&accountId=${accountId}&${fields}`;
    } 

    return new Promise((resolve, reject) => {
      
      this._httpClient.get(url)
          .subscribe((response: any) => {
              this.rooms = response.payload;
              this.onRoomsChanged.next(this.rooms);
              resolve(response);
          }, reject);
    })

    
  }


}
