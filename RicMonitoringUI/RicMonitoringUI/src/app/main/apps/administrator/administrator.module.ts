import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FuseSharedModule } from '@fuse/shared.module';
import { FuseWidgetModule, FuseDemoModule } from '@fuse/components';
import { MaterialModule } from 'app/main/module/material.module';
import { MatSelectCountryModule } from '@angular-material-extensions/select-country'
import { AdministratorShowErrorsComponent } from './administrator-show-errors.component';
import { DailyBatchComponent } from './daily-batch/daily-batch.component';
import { DailyBatchService } from './daily-batch/daily-batch.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from 'app/core/http-interceptors/auth-interceptor';

const routes : Routes = [
  {
    path          : 'daily-batch',
    component : DailyBatchComponent,
    resolve: {
      data: DailyBatchService
    }
  },
]

@NgModule({
  imports: [
    FuseSharedModule,
    RouterModule.forChild(routes),
    FuseWidgetModule,
    MaterialModule,
    FuseDemoModule,
    MatSelectCountryModule
  ],
  declarations: [
    AdministratorShowErrorsComponent,
    DailyBatchComponent
  ],
  providers: [
    DailyBatchService,
    {
			provide: HTTP_INTERCEPTORS,
			useClass: AuthInterceptor,
			multi: true
		}
  ]
})
export class AdministratorModule { }
