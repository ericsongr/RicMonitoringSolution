import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject } from 'rxjs';

const API_URL = `${environment.webApi}${ApiControllers.AppFunctions}`;

@Injectable()
export class AppFunctionsService {

onDaysWithSuffixChanged: BehaviorSubject<any> = new BehaviorSubject({});

constructor(private _httpClient: HttpClient) { }

  getDaysWithSuffix(selectedDate: string) : Promise<any> {
    
    return new Promise((resolve, reject) => {

      var url = `${API_URL}?selectedDate=${selectedDate}`;
      
      this._httpClient.get(url)
          .subscribe((daysWithSuffix: any) => {

            this.onDaysWithSuffixChanged.next(daysWithSuffix);
            
            resolve(daysWithSuffix);

          }, reject);
    
        });
  }

}
