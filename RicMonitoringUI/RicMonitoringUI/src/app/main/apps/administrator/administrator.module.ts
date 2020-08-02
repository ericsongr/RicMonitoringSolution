import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { TenantShowErrorsComponent } from '../common/tenant-show-errors.component';

import { UserComponent } from './user/user.component';
import { UserService } from './user/user.service';

import { UsersComponent } from './users/users.component';
import { UsersService } from './users/users.service';

import { FuseSharedModule } from '@fuse/shared.module';
import { FuseWidgetModule, FuseDemoModule } from '@fuse/components';
import { MaterialModule } from 'app/main/module/material.module';
import { MatSelectCountryModule } from '@angular-material-extensions/select-country'

const routes : Routes = [
  {
    path      : 'users',
    component : UsersComponent,
    resolve : {
      data  : UsersService
    },
  },
  {
    path      : 'users/:id',  
    component : UserComponent,
    resolve : {
      data  : UserService
    },
  },
  {
    path      : 'users/:id/:handle',
    component : UserComponent,
    resolve : {
      data  : UserService
    },
  }
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
  providers: [
    UserService,
    UsersService
  ],
  declarations: [
    TenantShowErrorsComponent,
    UserComponent,
    UsersComponent
  ]
})
export class AdministratorModule { }
