import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiControllers } from 'environments/api-controllers';
import { HttpClient } from '@angular/common/http';

const TABLE_FIELDS = "?fields=auditRoomId,id,name,frequency,price,auditDateTimeString,username,auditAction&orderBy=name";

@Injectable()
export class AuditRoomsService implements Resolve<any> {

  auditRooms: any[];
  onAuditRoomsChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _http: HttpClient,
    @Inject('API_URL') private apiUrl: string)  
  { }
  
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
      var url = `${this.apiUrl}${ApiControllers.Audits}/0/rooms${TABLE_FIELDS}`;
      return new Promise((resolve, reject) => {

        this._http.get(url)
            .subscribe((response: any) => {
                this.auditRooms = response.payload;
                this.onAuditRoomsChanged.next(this.auditRooms);
                resolve(this.auditRooms);
            }, reject);

    })
  }

}
