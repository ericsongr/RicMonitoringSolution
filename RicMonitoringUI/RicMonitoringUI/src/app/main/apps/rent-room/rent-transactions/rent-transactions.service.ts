import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiControllers } from 'environments/api-controllers';
import { HttpClient } from '@angular/common/http';

const TABLE_FIELDS = "fields=id,renterName,renterId,roomName,roomId,monthlyRent,dueDateString,datePaidString,paidAmount,balance,balanceDateToBePaid,totalAmountDue,isDepositUsed,transactionType,note,billingStatement&orderBy=dueDay";

@Injectable()
export class RentTransactionsService implements Resolve<any> {

  apiUrl: string;
  rentTransactions: any[];
  onRentTransactionsChanged: BehaviorSubject<any> = new BehaviorSubject({});

  routeParams: any;
  
  constructor(
    private _http: HttpClient,
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
      var url = `${this.apiUrl}?${TABLE_FIELDS}`;
      
      this._http.get(url)
          .subscribe((response: any) => {

              this.rentTransactions = response.payload;
              
              this.onRentTransactionsChanged.next(this.rentTransactions);
            
              resolve(response);

          }, reject);
    })

    
  }


}
