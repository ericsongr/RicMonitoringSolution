import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChangePasswordComponent } from './change-password.component';
import { RouterModule } from '@angular/router';
import { MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatSnackBar, MatSnackBarModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { UserDataService } from '../../users/user-data.service';
import { UsersService } from '../../users/users.service';
import { ChangePasswordShowErrorsComponent } from './change-password-show-errors.component';

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
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    ReactiveFormsModule
  ],
  providers: [
    UsersService, 
    UserDataService],
  declarations: [
    ChangePasswordComponent,
    ChangePasswordShowErrorsComponent]
})
export class ChangePasswordModule { }