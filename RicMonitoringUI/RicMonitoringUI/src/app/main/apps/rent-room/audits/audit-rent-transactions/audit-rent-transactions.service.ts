import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiControllers } from 'environments/api-controllers';
import { AuthService } from '../../../common/core/auth/auth.service';

const TABLE_FIELDS = "?fields=auditRentTransactionId,id,paidDateString,paidAmount,balance," +
                             "balanceDateToBePaidString,isDepositUsed,note,roomName,renterName," + 
                             "dueDateString,period,transactionType,isSystemProcessed,systemDateTimeProcessedString," + 
                             "totalAmountDue,isProcessed,excessPaidAmount,auditDateTimeString,username,auditAction&orderBy=auditDateTimeString";

@Injectable()
export class AuditRentTransactionsService implements Resolve<any> {

  auditRentTransactions: any[];
  onAuditRentTransactionsChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _authService: AuthService,
    @Inject('API_URL') private apiUrl: string)  
  { }
  
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
      var url = `${this.apiUrl}${ApiControllers.Audits}/0/transactions${TABLE_FIELDS}`;
      return new Promise((resolve, reject) => {
        
        this._authService.get(url)
            .subscribe((response: any) => {
                this.auditRentTransactions = response;
                this.onAuditRentTransactionsChanged.next(this.auditRentTransactions);
                resolve(response);
            }, reject);

    })
  }

}
