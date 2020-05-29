import { Injectable, Inject } from '@angular/core';
import { AuthService } from '../../common/core/auth/auth.service';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject } from 'rxjs';
import { RentTransactionHistory } from './rent-transaction-history.model';

const TABLE_FIELDS = "&orderBy=DueDate&field=dueDateString,paidDateString,dateUsedDepositString,period,paidAmount,balanceDateToBePaid,monthlyRent,previousBalance,currentBalance,totalAmountDue,isDepositUsed,note,transactionType";

@Injectable()
export class RentTransactionHistoryService {

  routeParams: any;
  onRentTransactionHistoryChanged: BehaviorSubject<any> = new BehaviorSubject({});

    constructor(
      private _authServer: AuthService,
      @Inject("API_URL") private _apiUrl: string) {}

      getHistory(renterId: number) : Promise<RentTransactionHistory[]> {
        return new Promise((resolve, reject) => {

          var url = `${this._apiUrl}${ApiControllers.RentTransactionHistory}?id=${renterId}${TABLE_FIELDS}`;
          this._authServer.get(url)
              .subscribe((response: RentTransactionHistory[]) => {
                resolve(response);
              }, reject);
        });
      
      }

}
