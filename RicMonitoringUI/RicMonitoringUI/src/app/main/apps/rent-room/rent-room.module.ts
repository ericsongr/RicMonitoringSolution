
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
import { RentTransactionPaymentComponent } from './rent-transaction/rent-transaction-payment/rent-transaction-payment.component';
import { AuditsModule } from './audits/audits-api';
import { DailyBatchComponent } from '../administrator/daily-batch/daily-batch.component';
import { DailyBatchService } from '../administrator/daily-batch/daily-batch.service';
import { DialogPaymentsComponent } from './renter/rent-transaction-history/dialog-payments/dialog-payments.component';
import { AccountsService } from '../administrator/accounts/accounts.service';

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
      },
      {
        path      : 'payment-history',
        component : RentTransactionHistoryComponent,
          resolve: {
            data: RentTransactionHistoryService
          },
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
  {
    path          : 'audit',
    loadChildren  : () => import('../rent-room/audits/audits.module').then(m => m.AuditsModule)
  },
]

@NgModule({
  imports: [
    FuseSharedModule,
    RouterModule.forChild(routes),
    FuseWidgetModule,
    MaterialModule,
    CopyClipboardModule,
    AuditsModule
  ],
  declarations: [
    DialogDeletePaymentConfirmationComponent,
    RoomsComponent,
    RoomComponent,
    RentersComponent,
    RenterComponent,
    RentTransactionsComponent,
    RentTransactionComponent,
    RentTransactionPaymentComponent,
    RentTransactionHistoryComponent,
    BillingStatementComponent,
    TenantShowErrorsComponent,
    RenterDetailComponent,
    RenterTabsComponent,
    DialogPaymentsComponent
  ],
  providers: [
    // { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    // { provide: MAT_DATE_FORMATS, useValue: US_FORMATS },
    // { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: {strict: true}},
    RoomsService,
    RoomService,
    RentersService,
    RenterDetailService,
    RentTransactionsService,
    RentTransactionService,
    RentTransactionHistoryService,
    AppFunctionsService,
    AccountsService
    // {
    //   provide: HTTP_INTERCEPTORS,
    //   useClass: AuthInterceptor,
    //   multi: true
    // }
  ],
  entryComponents: [
    DialogDeletePaymentConfirmationComponent,
    DialogPaymentsComponent
  ]
})
export class RentRoomModule { 

}
