import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { MatSnackBar } from '@angular/material';
import { FuseUtils } from '@fuse/utils';
import { Location } from '@angular/common';
import { fuseAnimations } from '@fuse/animations';

import { UserProfileService } from './user-profile.service';

import { UserProfile } from './user-profile.model';
import { Router } from '@angular/router';
import { Role } from '../user/role.model';


@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
  animations: [fuseAnimations]
})
export class UserProfileComponent implements OnInit, OnDestroy {
  
  user = new UserProfile();
  roles: Role[];
  pageType: string;
  
  userEntryForm: FormGroup;

  onUserChangedSubscription: Subscription;
  
  displayedColumns = ['deviceId', 'platform', 'lastAccessOnUtc'];

  constructor(
    private _userService: UserProfileService,
    private _formBuilder: FormBuilder,
    private _snackBar : MatSnackBar,
    private _location : Location,
    private _router: Router
    ) { }
    
    ngOnInit() {
      
      this.onUserChangedSubscription =
        this._userService.onUserChanged
            .subscribe(response => {
              
              this.user = new UserProfile(response.payload);
              
              //get all roles
              this.getRoles();
              
              this.userEntryForm = this.updateUserForm();
              
            });
    }

  getRoles() {
    this._userService.getRoles()
        .then((response: Role[]) => {
          this.roles = response;
        })
  }

  updateUserForm(): FormGroup {
    return this._formBuilder.group({
      id            : [this.user.id],
      firstName     : [this.user.firstName, Validators.required],
      lastName      : [this.user.lastName, Validators.required],
      email         : [this.user.email, Validators.required],
      mobileNumber  : [this.user.mobileNumber, Validators.required],
      phoneNumber   : [this.user.phoneNumber],
      userName      : [this.user.userName, Validators.required],
      role          : [this.user.role],
      isIncomingDueDatePushNotification       : [this.user.isIncomingDueDatePushNotification],
      isReceiveDueDateAlertPushNotification   : [this.user.isReceiveDueDateAlertPushNotification],
      isPaidPushNotification                  : [this.user.isPaidPushNotification],
    });
  }
  
  save() {

    if (this.userEntryForm.invalid) {

      //show the success message
      this._snackBar.open('Invalid data. Please verify.', 'OK', {
        verticalPosition: 'top',
        panelClass: ['mat-warn']
      });

    } else {
      
      var formData = this.userEntryForm.getRawValue();
      formData.handle = FuseUtils.handleize(formData.lastName);

      this._userService.saveUser(formData)
        .then(() => {

          //Trigger the subscription with new data
          this._userService.onUserChanged.next(formData);

          //show the success message
          this._snackBar.open('User detail saved.', 'OK', {
            verticalPosition: 'top',
            duration: 2000
          });
          debugger;
          //change the location with new one
          this._router.navigate([`/administrator/users`]);
          // this._location.go(`/administrator/users`);
        });
    } 
  }

  ngOnDestroy(): void {
    this.onUserChangedSubscription.unsubscribe();
  }
}
