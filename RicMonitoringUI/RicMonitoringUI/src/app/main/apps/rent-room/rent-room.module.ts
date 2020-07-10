
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
import { RenterDetailComponent } from './renter/details/renter-details.component';
import { RenterDetailService } from './renter/details/renter-details.service';
import { RentTransactionsComponent } from './rent-transactions/rent-transactions.component';
import { RentTransactionsService } from './rent-transactions/rent-transactions.service';
import { RentTransactionService } from './rent-transaction/rent-transaction.service';
import { AppFunctionsService } from '../common/services/app-functions.service';
import { RentTransactionComponent } from './rent-transaction/rent-transaction.component';
import { CopyClipboardModule } from '../common/directives/copy-clipboard.module';
import { BillingStatementComponent } from './rent-transactions/billing-statement/billing-statement.component';
import { RentTransactionHistoryService } from './renter/rent-transaction-history/rent-transaction-history.service';
import { RentTransactionHistoryComponent } from './renter/rent-transaction-history/rent-transaction-history.component';
import { DialogDeletePaymentConfirmationComponent } from './rent-transaction/dialog-delete-payment-confirmation/dialog-delete-payment-confirmation.component';
import { RenterComponent } from './renter/renter.component';
import { RenterTabsComponent } from './renter/renter-tabs/renter-tabs.component';
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
    children: [
      { path: '', redirectTo: 'details', outlet: 'tab', pathMatch: 'full' },
      {
        path      : 'details',
        component : RenterDetailComponent,
          resolve: {
            data: RenterDetailService
          },
          outlet    : 'tab',
      }
    ]
  },
  {
    path      : 'tenants/:id/:handle',
    component : RenterComponent,
    children: [
      { path: '', redirectTo: 'details', outlet: 'tab', pathMatch: 'full' },
      {
        path      : 'details',
        component : RenterDetailComponent,
          resolve: {
            data: RenterDetailService
          },
          outlet    : 'tab',
        // pathMatch: 'full'
      },
      {
        path      : 'payment-history',
        component : RentTransactionHistoryComponent,
          resolve: {
            data: RentTransactionHistoryService
          },
        // pathMatch: 'full'
        outlet    : 'tab',
      }
    ]
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
  },
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
    TenantShowErrorsComponent,
    RenterDetailComponent,
    RenterTabsComponent
  ],
  providers: [
    RoomsService,
    RoomService,
    RentersService,
    RenterDetailService,
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
    DialogDeletePaymentConfirmationComponent
  ]
})
export class RentRoomModule { 

}
