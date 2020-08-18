import { Injectable } from '@angular/core';
import { UserData } from './user-data';

@Injectable()
export class UserDataService {

userData = new UserData();

  constructor() { 
    
    var data = JSON.parse(sessionStorage['userData_spaRicMonitoringCodeClient'])
    this.userData.sub = data.sub;
    this.userData.username = data.preferred_username;
    this.userData.name = data.name;
    this.userData.fullName = data.FullName;
    this.userData.role = data.role;
  }

  getUserData() {
    return this.userData;
  }

  getSub() {
    return this.userData.sub;
  }
  
  getUsername() {
    return this.userData.username;
  }

  getName() {
    return this.userData.name;
  }
  
  getFullname() {
    return this.userData.fullName;
  }

  getRole() {
    return this.userData.role;
  }

}
