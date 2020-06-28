
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoomsComponent } from './rooms/rooms.component';
import { RoomComponent } from './room/room.component';
import { RoomsService } from './rooms/rooms.service';
import { RoomService } from './room/room.service';
import { FuseSharedModule } from '@fuse/shared.module';
import { FuseWidgetModule } from '@fuse/components';
import { MaterialModule } from 'app/main/module/material.module';
import { TenantShowErrorsComponent } from '../common/tenant-show-errors.component';
import { RentersComponent } from './renters/renters.component';
import { RentersService } from './renters/renters.service';
import { RenterComponent } from './renter/renter.component';
import { RenterService } from './renter/renter.service';
import { RentTransactionsComponent } from './rent-transactions/rent-transactions.component';
import { RentTransactionsService } from './rent-transactions/rent-transactions.service';
import { RentTransactionService } from './rent-transaction/rent-transaction.service';
import { AppFunctionsService } from '../common/services/app-functions.service';
import { RentTransactionComponent } from './rent-transaction/rent-transaction.component';
import { CopyClipboardModule } from '../common/directives/copy-clipboard.module';
import { BillingStatementComponent } from './rent-transactions/billing-statement/billing-statement.component';
import { RentTransactionHistoryService } from './rent-transaction-history/rent-transaction-history.service';
import { RentTransactionHistoryComponent } from './rent-transaction-history/rent-transaction-history.component';
import { DialogDeletePaymentConfirmationComponent } from './rent-transaction/dialog-delete-payment-confirmation/dialog-delete-payment-confirmation.component';
// import { HTTP_INTERCEPTORS } from '@angular/common/http';
// import { AuthInterceptor } from '../common/core/http-interceptor/AuthInterceptor';

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
    path      : 'tenant-transactions/:id/:handle',
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
    MaterialModule,
    CopyClipboardModule
  ],
  declarations: [
    DialogDeletePaymentConfirmationComponent,
    RoomsComponent,
    RoomComponent,
    RentersComponent,
    RenterComponent,
    RentTransactionsComponent,
    RentTransactionComponent,
    RentTransactionHistoryComponent,
    BillingStatementComponent,
    TenantShowErrorsComponent
  ],
  providers: [
    RoomsService,
    RoomService,
    RentersService,
    RenterService,
    RentTransactionsService,
    RentTransactionService,
    RentTransactionHistoryService,
    AppFunctionsService,
    // {
    //   provide: HTTP_INTERCEPTORS,
    //   useClass: AuthInterceptor,
    //   multi: true
    // }
  ],
  entryComponents: [
    DialogDeletePaymentConfirmationComponent,
    RentTransactionHistoryComponent
  ]
})
export class RentRoomModule { 

}
