
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomsComponent } from './rooms/rooms.component';
import { RoomComponent } from './room/room.component';
import { RoomsService } from './rooms/rooms.service';
import { RoomService } from './room/room.service';
import { FuseSharedModule } from '@fuse/shared.module';
import { FuseWidgetModule } from '@fuse/components';
import { MaterialModule } from 'app/main/module/material.module';
import { ShowErrorsComponent } from '../common/show-errors.component';
import { RentersComponent } from './renters/renters.component';
import { RentersService } from './renters/renters.service';
import { RenterComponent } from './renter/renter.component';
import { RenterService } from './renter/renter.service';
import { RentTransactionsComponent } from './rent-transactions/rent-transactions.component';
import { RentTransactionsService } from './rent-transactions/rent-transactions.service';
import { RentTransactionService } from './rent-transaction/rent-transaction.service';
import { AppFunctionsService } from '../common/services/app-functions.service';
import { RentTransactionComponent } from './rent-transaction/rent-transaction.component';

const routes : Routes= [
  {
    path      :  'rooms',
    component :  RoomsComponent,
    resolve   : {
          data: RoomsService
    }
  },
  {
    path      : 'rooms/:id',
    component : RoomComponent,
    resolve: {
      data: RoomService
    }
  },
  {
    path      : 'rooms/:id/:handle',
    component : RoomComponent,
    resolve: {
      data: RoomService
    }
  },
  {
    path      :  'tenants',
    component :  RentersComponent,
    resolve   : {
          data: RentersService
    }
  },
  {
    path      : 'tenants/:id',
    component : RenterComponent,
    resolve: {
      data: RenterService
    }
  },
  {
    path      : 'tenants/:id/:handle',
    component : RenterComponent,
    resolve: {
      data: RenterService
    }
  },
  //rent transactions
  {
    path      :  'tenant-transactions',
    component :  RentTransactionsComponent,
    resolve   : {
          data: RentTransactionsService
    }
  },
  {
    path      : 'tenant-transactions/:renterId/:handle',
    component : RentTransactionComponent,
    resolve: {
      data: RentTransactionService
    }
  }
]

@NgModule({
  imports: [
    FuseSharedModule,
    RouterModule.forChild(routes),
    FuseWidgetModule,
    MaterialModule
  ],
  declarations: [
    RoomsComponent,
    RoomComponent,
    RentersComponent,
    RenterComponent,
    RentTransactionsComponent,
    RentTransactionComponent,
    ShowErrorsComponent
  ],
  providers: [
    RoomsService,
    RoomService,
    RentersService,
    RenterService,
    RentTransactionsService,
    RentTransactionService,
    AppFunctionsService
  ]
})
export class RentRoomModule { 

}
