import { NgModule } from '@angular/core';
import { AuditsComponent } from './audits.component';
import { AuditsRoutingModule } from './audits-routing.module';
import { AuditTabsComponent } from './audit-tabs/audit-tabs.component';
import { FuseSharedModule } from '@fuse/shared.module';
import { FuseWidgetModule } from '@fuse/components';
import { MaterialModule } from 'app/main/module/material.module';
import { AuditRoomsComponent } from './audit-rooms/audit-rooms.component';
import { AuditRentersComponent } from './audit-renters/audit-renters.component';
import { AuditRentTransactionsComponent } from './audit-rent-transactions/audit-rent-transactions.component';
import { AuditRoomsService } from './audit-rooms/audit-rooms.service';

@NgModule({
  imports: [
    FuseSharedModule,
    FuseWidgetModule,
    MaterialModule,
    AuditsRoutingModule
  ],
  exports: [
    AuditsComponent
  ],
  declarations: [
    AuditsComponent,
    AuditTabsComponent,
    AuditRoomsComponent,
    AuditRentersComponent,
    AuditRentTransactionsComponent
  ],
  providers: [
    AuditRoomsService
  ]
})
export class AuditsModule { }
