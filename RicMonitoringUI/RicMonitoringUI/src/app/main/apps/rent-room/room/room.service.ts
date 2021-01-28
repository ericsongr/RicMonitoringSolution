import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';


@Injectable()
export class RoomService implements Resolve<any> 
{

  apiUrl: string;
  routeParams: any;
  room: any;
  onRoomChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _httpClient: HttpClient,
    @Inject('API_URL') private _apiUrl: string
  ) {
    this.apiUrl = `${_apiUrl}${ApiControllers.Rooms}/`;
   }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    this.routeParams = route.params;

    return new Promise((resolve, reject) => {
      Promise.all([
        this.getRoom()
      ]).then(() => {
        resolve();
      }, reject)
    });
  }

  getRoom(): Promise<any> {
    return new Promise((resolve, reject) => {

      if (this.routeParams.id === 'new') {
        this.onRoomChanged.next(false);
        resolve(false);
      }
      else 
      {
        this._httpClient.get(this.apiUrl + this.routeParams.id)
            .subscribe((response: any) => {
                this.room = response.payload;
                
                this.onRoomChanged.next(this.room);
                resolve(response);
            }, reject);
      }

    });
  }

  saveRoom(room){
    return new Promise((resolve, reject) => {
      this._httpClient.put(this.apiUrl + room.id, room)
          .subscribe((response: any) => {
            resolve(response);
          }, reject);
    });
  }

  addRoom(room){
    return new Promise((resolve, reject) => {
      this._httpClient.post(this.apiUrl, room)
          .subscribe((response: any) => {
            resolve(response);
          }, reject);
    });
  }

}
