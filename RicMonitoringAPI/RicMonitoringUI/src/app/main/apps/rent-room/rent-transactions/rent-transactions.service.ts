import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';

const API_URL = environment.webApi + ApiControllers.RentTransactions;
const TABLE_FIELDS = "?fields=id,renterName,renterId,roomName,roomId,monthlyRent,dueDateString,datePaidString,paidAmount,balance,balanceDateToBePaid,totalAmountDue,isDepositUsed,transactionType,note&orderBy=dueDay";

@Injectable()
export class RentTransactionsService implements Resolve<any> {

  rentTransactions: any[];
  onRentTransactionsChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(private _httpClient: HttpClient)  
  { 
    
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

      var url = `${API_URL}${TABLE_FIELDS}`;
      console.log(url);
      this._httpClient.get(url)
          .subscribe((response: any) => {
              this.rentTransactions = response;
            
              this.onRentTransactionsChanged.next(this.rentTransactions);
            
              resolve(response);

          }, reject);
    })

    
  }


}
