import { NgModule, Component } from '@angular/core';
import { Route, RouterModule } from '@angular/router';

import { AuditsComponent } from './audits.component';
import { AuditRoomsComponent } from './audit-rooms/audit-rooms.component';
import { AuditRentersComponent } from './audit-renters/audit-renters.component';

const routes : Route[] = [
  {
    path        : 'logs',
    component   : AuditsComponent,
    children: [
      {
        path      : 'rooms',
        component : AuditRoomsComponent,
          // resolve: {
          //   data: RenterDetailService
          // },
          outlet    : 'tab',
      },
      {
        path      : 'renters',
        component : AuditRentersComponent,
          // resolve: {
          //   data: RentTransactionHistoryService
          // },
        outlet    : 'tab',
      },
      {
        path      : 'rent-transactions',
        component : AuditRentersComponent,
          // resolve: {
          //   data: RentTransactionHistoryService
          // },
        outlet    : 'tab',
      },
      { path: '', redirectTo: 'rooms', outlet: 'tab', pathMatch: 'full' },
    ]
  }
]

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AuditsRoutingModule { }
