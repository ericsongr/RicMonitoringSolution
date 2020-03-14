import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { LookupTypeItemsService } from '../../common/services/lookup-type-items.service';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable()
export class OnlineBookingService {

onLookupTypeItemsChanged: BehaviorSubject<any> = new BehaviorSubject({});

constructor(
    private _lookupTypeItemsService: LookupTypeItemsService) 
    { }

      
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getAges()
      ]).then(() => {
        resolve();
      }, reject);
    });
  }

    getAges() {
        this._lookupTypeItemsService
            .getItemsByLookupType('ages')
                .then(result => {
                    this.onLookupTypeItemsChanged.next(result);
                }).catch(error => {
                    console.log('Error on function GetLookupTypeItem - Ages')
                });
    }

}

  