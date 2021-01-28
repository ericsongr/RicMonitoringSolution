import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiControllers } from 'environments/api-controllers';
import { HttpClient } from '@angular/common/http';

const TABLE_FIELDS = "?fields=id,name,advancePaidDateString,startDateString,dueDayString,noOfPersons,roomName,advanceMonths,monthsUsed,dateEndString, isEndRent, balanceAmount, balancePaidDateString, totalPaidAmount, nextDueDateString, previousDueDateString, AuditDateTimeString, username,auditAction&orderBy=dueDay";

@Injectable()
export class AuditRentersService implements Resolve<any> {

  auditRenters: any[];
  onAuditRentersChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _http: HttpClient,
    @Inject('API_URL') private apiUrl: string)  
  { }
  
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
      var url = `${this.apiUrl}${ApiControllers.Audits}/0/renters${TABLE_FIELDS}`;
      return new Promise((resolve, reject) => {

        this._http.get(url)
            .subscribe((response: any) => {
                this.auditRenters = response.payload;
                this.onAuditRentersChanged.next(this.auditRenters);
                resolve(this.auditRenters);
            }, reject);

    })
  }

}
