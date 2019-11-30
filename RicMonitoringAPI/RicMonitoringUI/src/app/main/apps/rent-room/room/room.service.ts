import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { environment } from 'environments/environment';
import { ApiControllers } from 'environments/api-controllers';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

const API_URL = environment.webApi + ApiControllers.Rooms + "/";

@Injectable()
export class RoomService implements Resolve<any> 
{

  routeParams: any;
  room: any;
  onRoomChanged: BehaviorSubject<any> = new BehaviorSubject({});

  constructor(
    private _httpClient :HttpClient
  ) { }

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
        this._httpClient.get(API_URL + this.routeParams.id)
            .subscribe((response: any) => {
                this.room = response;
                this.onRoomChanged.next(this.room);
                resolve(response);
            }, reject);
      }

    });
  }

  saveRoom(room){
    return new Promise((resolve, reject) => {
      this._httpClient.put(API_URL + room.id, room)
          .subscribe((response: any) => {
            resolve(response);
          }, reject);
    });
  }

  addRoom(room){
    return new Promise((resolve, reject) => {
      this._httpClient.post(API_URL, room)
          .subscribe((response: any) => {
            resolve(response);
          }, reject);
    });
  }

}
