import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path          : '',
    loadChildren  : './rent-room/rent-room.module#RentRoomModule'
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
  declarations: []
})
export class AppRicMonitoringModule { }
