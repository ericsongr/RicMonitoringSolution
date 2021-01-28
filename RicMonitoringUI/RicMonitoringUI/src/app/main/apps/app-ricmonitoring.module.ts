import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthInterceptor } from 'app/core/http-interceptors/auth-interceptor';

const routes: Routes = [
  {
    path          : '',
    loadChildren  : () => import('./rent-room/rent-room.module').then(m => m.RentRoomModule)
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
  declarations: [],
  providers: [
    {
			provide: HTTP_INTERCEPTORS,
			useClass: AuthInterceptor,
			multi: true
		}
  ]
})
export class AppRicMonitoringModule { }
