import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { RentTransaction } from './rent-transaction.model';

const API_URL = environment.webApi + ApiControllers.RentTransactions + "/";
const fields = "id,renterName,renterId,roomName,roomId,monthlyRent,dueDate,dueDateString,period,paidDate,paidAmount,balance,balanceDateToBePaid,previousUnpaidAmount,rentArrearId,totalAmountDue,isDepositUsed,note,transactionType";

@Injectable()
export class RentTransactionService implements Resolve<any> 
{

  routeParams: any;
  rentTransaction: any;
  onRentTransactionChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _httpClient :HttpClient
  ) { }

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
    
    var url = `${API_URL}${this.routeParams.renterId}?fields=${fields}`;
    
    return new Promise((resolve, reject) => {

      this._httpClient.get(url)
            .subscribe((response: any) => {
                this.rentTransaction = response;
                console.log(response);
                this.onRentTransactionChanged.next(response);
                resolve(response);
            }, reject);

    });
  }

  saveTransaction(transaction: RentTransaction) {
      
      return new Promise((resolve, reject) => {
        if (transaction.id > 0) {
            var url = API_URL + transaction.id;
            console.log(url);
            console.log(JSON.stringify(transaction));

            this._httpClient.put(API_URL + transaction.id, transaction)
                .subscribe((response: any) => {
                  //return transactionId
                  resolve(response.id);
                }, reject);

          }
          else {

            this._httpClient.post(API_URL, transaction)
                    .subscribe((response: any) => {
                      //return transactionId
                      resolve(response.id);
                    }, reject);
      
          }
      });

   
  }


}
