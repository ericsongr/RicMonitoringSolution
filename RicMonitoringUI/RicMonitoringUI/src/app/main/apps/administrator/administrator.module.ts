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
import { AccountsComponent } from './accounts/accounts.component';
import { AccountsService } from './accounts/accounts.service';
import { AccountComponent } from './account/account.component';
import { AccountService } from './account/account.service';
import { GooglePlacesDirective } from '../common/directives/google-places.directive';
import { SettingsComponent } from './settings/settings.component';
import { SettingsService } from './settings/settings.service';
import { EditSettingDialogComponent } from './settings/edit-setting-dialog/edit-setting-dialog.component';
import { UsersService } from './users/users.service';
import { UsersComponent } from './users/users.component';
import { UserComponent } from './user/user.component';
import { UserService } from './user/user.service';

const routes : Routes = [
  {
    path      : 'accounts',
    component : AccountsComponent,
    resolve: {
      data: AccountsService
    }
  },
  {
    path      : 'account/:id',
    component : AccountComponent,
    resolve: {
      data: AccountService
    }
  },
  {
    path      : 'account/:id/:handle',
    component : AccountComponent,
    resolve: {
      data: AccountService
    }
  },
  {
    path      : 'users',
    component : UsersComponent,
    resolve: {
      data: UsersService
    }
  },
  {
    path      : 'users/:id',
    component : UserComponent,
    resolve: {
      data: UserService
    }
  },
  {
    path      : 'users/:id/:handle',
    component : UserComponent,
    resolve: {
      data: UserService
    }
  },
  {
    path      : 'daily-batch',
    component : DailyBatchComponent,
    resolve: {
      data: DailyBatchService
    }
  },
  {
    path      : 'settings',
    component : SettingsComponent,
    resolve: {
      data: SettingsService
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
    AccountComponent,
    AccountsComponent,
    DailyBatchComponent,
    SettingsComponent,
    EditSettingDialogComponent,
    GooglePlacesDirective,
    UsersComponent,
    UserComponent,
  ],
  entryComponents: [
    EditSettingDialogComponent
  ],
  providers: [
    DailyBatchService,
    AccountService,
    AccountsService,
    SettingsService,
    UsersService,
    UserService,
    {
			provide: HTTP_INTERCEPTORS,
			useClass: AuthInterceptor,
			multi: true
		}
  ]
})
export class AdministratorModule { }
