import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { RentTransaction } from './rent-transaction.model';
import { AuthService } from '../../common/core/auth/auth.service';

const fields = "id,renterName,renterId,roomName,roomId," +
               "monthlyRent,dueDate,dueDateString,period,paidDate," +
               "paidAmount,balance,balanceDateToBePaid,previousUnpaidAmount," + 
               "rentArrearId,totalAmountDue,isDepositUsed,note,transactionType," + 
               "isNoAdvanceDepositLeft,isProcessed," +
               "billingStatement,payments";

@Injectable()
export class RentTransactionService implements Resolve<any> 
{
  apiUrl: string;

  routeParams: any;
  rentTransaction: any;
  onRentTransactionChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _authService : AuthService,
    @Inject('API_URL') private _apiUrl: string) 
  {
    this.apiUrl = `${this._apiUrl}${ApiControllers.RentTransactions}/`
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    this.routeParams = route.params;
    
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getRentTransaction()
      ]).then(() => {
        resolve();
      }, reject)
    });
  }

  getRentTransaction(): Promise<any> {
    
    var url = `${this.apiUrl}${this.routeParams.renterId}/${this.routeParams.monthFilter}?fields=${fields}`;
    
    return new Promise((resolve, reject) => {

      this._authService.get(url)
            .subscribe((response: any) => {

                this.rentTransaction = response;
                this.onRentTransactionChanged.next(response);
                resolve(response);
            }, reject);

    });
  }

  saveTransaction(transaction: RentTransaction) {
      return new Promise((resolve, reject) => {
        
        if (transaction.id > 0) {
            var url = this.apiUrl + transaction.id;
            this._authService.put(url, transaction)
                .subscribe((response: any) => {
                  
                  resolve(response.id);
                }, reject);

          }
          else {

            this._authService.post(this.apiUrl, transaction)
                    .subscribe((response: any) => {
                      
                      resolve(response.id);
                    }, reject);
      
          }
      });

   
  }

  deletePayment(id) {

    var url = `${this._apiUrl}${ApiControllers.RentTransactionPayments}/${id}`
    return new Promise((resolve, reject) => {
      this._authService.delete(url)
        .subscribe((response: any) => {
          resolve(response);
        }, reject);
    });
    
  }

}
