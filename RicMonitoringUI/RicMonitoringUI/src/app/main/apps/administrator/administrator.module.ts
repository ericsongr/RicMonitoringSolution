import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './users/users.component';
import { UsersService } from './users/users.service';
import { FuseSharedModule } from '@fuse/shared.module';
import { FuseWidgetModule, FuseDemoModule } from '@fuse/components';
import { MaterialModule } from 'app/main/module/material.module';
import { MatSelectCountryModule } from '@angular-material-extensions/select-country';

const routes : Routes = [
  {
    path      : 'users',
    component : UsersComponent,
    resolve : {
      data  : UsersService
    }
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
  providers: [UsersService],
  declarations: [UsersComponent]
})
export class AdministratorModule { }
