import { Injectable } from '@angular/core';
import { UserData } from './user-data';

@Injectable()
export class UserDataService {

userData = new UserData();

  constructor() { 
    
    var data = JSON.parse(sessionStorage['accessData'])
    this.userData.username = data.username;
    this.userData.name = data.name;
    this.userData.role = data.role;
  }

  getUserData() {
    return this.userData;
  }

  getUsername() {
    return this.userData.username;
  }

  getName() {
    return this.userData.name;
  }
  
  getRole() {
    return this.userData.role;
  }

}
