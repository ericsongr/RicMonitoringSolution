import { NgModule, Component } from '@angular/core';
import { Route, RouterModule } from '@angular/router';

import { AuditsComponent } from './audits.component';
import { AuditRoomsComponent } from './audit-rooms/audit-rooms.component';
import { AuditRentersComponent } from './audit-renters/audit-renters.component';
import { AuditRoomsService } from './audit-rooms/audit-rooms.service';
import { AuditRentersService } from './audit-renters/audit-renters.service';
import { AuditRentTransactionsService } from './audit-rent-transactions/audit-rent-transactions.service';
import { AuditRentTransactionsComponent } from './audit-rent-transactions/audit-rent-transactions.component';

const routes : Route[] = [
  {
    path        : 'logs',
    component   : AuditsComponent,
    children: [
      {
        path      : 'rooms',
        component : AuditRoomsComponent,
          resolve: {
            data: AuditRoomsService
          },
          outlet    : 'tab',
      },
      {
        path      : 'renters',
        component : AuditRentersComponent,
          resolve: {
            data: AuditRentersService
          },
        outlet    : 'tab',
      },
      {
        path      : 'rent-transactions',
        component : AuditRentTransactionsComponent,
          resolve: {
            data: AuditRentTransactionsService
          },
        outlet    : 'tab',
      },
      { path: '', redirectTo: 'rent-transactions', outlet: 'tab', pathMatch: 'full' },
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
