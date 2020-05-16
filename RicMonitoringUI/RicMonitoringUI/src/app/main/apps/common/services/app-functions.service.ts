import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class AppFunctionsService {

onDaysWithSuffixChanged: BehaviorSubject<any> = new BehaviorSubject({});

constructor(private _httpClient: HttpClient,
           @Inject('API_URL') private _apiUrl: string) { }

  getDaysWithSuffix(selectedDate: string) : Promise<any> {
    
    return new Promise((resolve, reject) => {

      var url = `${this._apiUrl}${ApiControllers.AppFunctions}?selectedDate=${selectedDate}`;
      
      this._httpClient.get(url)
          .subscribe((daysWithSuffix: any) => {

            this.onDaysWithSuffixChanged.next(daysWithSuffix);
            
            resolve(daysWithSuffix);

          }, reject);
    
        });
  }

}
