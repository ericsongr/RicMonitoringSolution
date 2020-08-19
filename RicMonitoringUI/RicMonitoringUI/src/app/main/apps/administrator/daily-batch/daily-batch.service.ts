import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiControllers } from 'environments/api-controllers';
import { AuthService } from '../../common/core/auth/auth.service';

@Injectable()
export class DailyBatchService implements Resolve<any> {

  dailyBatch: any[];
  onDailyBatchChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _authService: AuthService,
    @Inject('API_URL') private apiUrl: string)  
  { 
    
  }
  
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getDailyBatch()
      ]).then(() => {
        resolve();
      }, reject);
    });
  }

  getDailyBatch(): Promise<any> {

    var url = `${this.apiUrl}${ApiControllers.ExecStoreProc}?field=processStartDateTimeString,processsEndDateTimeString,duration`;

    return new Promise((resolve, reject) => {
      
      this._authService.get(url)
          .subscribe((response: any) => {
             debugger;
            this.dailyBatch = response;
             
              this.onDailyBatchChanged.next(this.dailyBatch);
            
              resolve(response);
          
            }, reject);
    })

    
  }

}
