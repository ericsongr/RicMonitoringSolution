import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';
import { AuthService } from '../../common/core/auth/auth.service';

const API_URL = environment.webApi + ApiControllers.Rooms;
const TABLE_FIELDS = "?fields=id,name,frequency,price&orderBy=name";
const DROPDOWN_FIELDS = "?fields=id,name,isOccupied,price&orderBy=name";

@Injectable()
export class RoomsService implements Resolve<any> {

  rooms: any[];
  onRoomsChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _httpClient: HttpClient,
    private _authService: AuthService)  
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

  getRooms(htmlComponent): Promise<any> {
    var fields = TABLE_FIELDS;
    if (htmlComponent == "dropdown"){
      fields = DROPDOWN_FIELDS;
    }

    return new Promise((resolve, reject) => {
      var url = `${API_URL}${fields}`;

      this._authService.get(url)
          .subscribe((response: any) => {
              this.rooms = response;
              this.onRoomsChanged.next(this.rooms);
              resolve(response);
          }, reject);
    })

    
  }


}
