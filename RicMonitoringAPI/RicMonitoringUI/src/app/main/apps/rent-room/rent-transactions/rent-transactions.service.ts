import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiControllers } from 'environments/api-controllers';
import { AuthService } from '../../common/core/auth/auth.service';

const TABLE_FIELDS = "?fields=id,renterName,renterId,roomName,roomId,monthlyRent,dueDateString,datePaidString,paidAmount,balance,balanceDateToBePaid,totalAmountDue,isDepositUsed,transactionType,note,billingStatement&orderBy=dueDay";

@Injectable()
export class RentTransactionsService implements Resolve<any> {

  apiUrl: string;
  rentTransactions: any[];
  onRentTransactionsChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _authService: AuthService,
    @Inject('API_URL') private _apiUrl: string)  
  { 
    this.apiUrl = `${_apiUrl}${ApiControllers.RentTransactions}`;
  }
  
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getRentTransactions()
      ]).then(() => {
        resolve();
      }, reject);
    });
  }

  getRentTransactions(): Promise<any> {
       return new Promise((resolve, reject) => {

      var url = `${this.apiUrl}${TABLE_FIELDS}`;
      
      this._authService.get(url)
          .subscribe((response: any) => {
              this.rentTransactions = response;
              
              this.onRentTransactionsChanged.next(this.rentTransactions);
            
              resolve(response);

          }, reject);
    })

    
  }


}
