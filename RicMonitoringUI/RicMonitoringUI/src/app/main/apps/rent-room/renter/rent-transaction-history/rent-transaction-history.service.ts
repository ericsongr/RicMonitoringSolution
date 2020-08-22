import { Injectable, Inject } from '@angular/core';
import { AuthService } from '../../../common/core/auth/auth.service';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject, Observable } from 'rxjs';
import { RentTransactionHistory } from './rent-transaction-history.model';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

const TABLE_FIELDS = "&orderBy=DueDate&field=dueDateString,paidOrUsedDepositDateString,"
                     "period,paidAmount,balanceDateToBePaidString,monthlyRent,previousBalance,"
                     "currentBalance,totalAmountDue,isDepositUsed,note,transactionType,payments";

@Injectable()
export class RentTransactionHistoryService implements Resolve<any> {

  routeParams: any;
  onRentTransactionHistoryChanged: BehaviorSubject<any> = new BehaviorSubject({});
  rentTransactionHistories: RentTransactionHistory[];
    constructor(
      private _authServer: AuthService,
      @Inject("API_URL") private _apiUrl: string) {}


  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) : Observable<any> | Promise<any> | any {
    this.routeParams = route.parent.params;
    return new Promise((resolve, reject) => {
      Promise.all([
       this.getHistory() 
      ]).then(() => {
        resolve();
      }, reject)
    });

  }

  getHistory() : Promise<RentTransactionHistory[]> {
    
    var renterId = this.routeParams.id;

    return new Promise((resolve, reject) => {

      var url = `${this._apiUrl}${ApiControllers.RentTransactionHistory}?id=${renterId}${TABLE_FIELDS}`;
      this._authServer.get(url)
          .subscribe((response: RentTransactionHistory[]) => {0
            this.onRentTransactionHistoryChanged.next(response);
            this.rentTransactionHistories = response;
            resolve(response);
          }, reject);
    });
  
  }

}
