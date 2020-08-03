import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChangePasswordComponent } from './change-password.component';
import { RouterModule } from '@angular/router';
import { MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule } from '@angular/material';

@NgModule({
  imports: [
    RouterModule.forChild([{
      path: '',
      component : ChangePasswordComponent
    }])
  ],
  exports: [RouterModule]
})
export class ChangePasswordRoutingModule {}

@NgModule({
  imports: [
    CommonModule,
    ChangePasswordRoutingModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  declarations: [ChangePasswordComponent]
})
export class ChangePasswordModule { }