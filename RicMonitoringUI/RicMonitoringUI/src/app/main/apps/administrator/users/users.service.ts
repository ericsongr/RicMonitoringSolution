import { Injectable, Inject } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';
import { ApiControllers } from 'environments/api-controllers';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class UsersService implements Resolve<any> {

  users: any[];
  onUsersChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _httpClient: HttpClient,
    @Inject('AUTH_URL') private authUrl: string)  
  { 
    
  }
  
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return new Promise((resolve, reject) => {
      Promise.all([
        this.getUsers()
      ]).then(() => {
        resolve();
      }, reject);
    });
  }

  getUsers(): Promise<any> {

    var url = `${this.authUrl}/api/${ApiControllers.Account}`;

    return new Promise((resolve, reject) => {
      
      var httpHeaders = this.getHeaders();
      
      this._httpClient.get(url, {headers: httpHeaders} )
          .subscribe((response: any) => {
             
            this.users = response.payload;
             
              this.onUsersChanged.next(this.users);
            
              resolve(response);
          
            }, reject);
    })

    
  }

  private getHeaders() {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json');
    headers = headers.set('Accept', 'application/json');
    return headers;
  }

}
