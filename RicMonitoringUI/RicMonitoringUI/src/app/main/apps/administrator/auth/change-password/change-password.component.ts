import { Component, OnInit, AfterViewInit, TemplateRef, ViewChild, ChangeDetectionStrategy, ViewEncapsulation, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { MatDialog, MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FormGroup, ValidatorFn, AbstractControl, ValidationErrors, Validators, FormBuilder } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserDataService } from '../../users/user-data.service';
import { UsersService } from '../../users/users.service';
import { ApiResponseError } from 'app/main/apps/common/models/api-response-error';
import * as _ from 'lodash'

@Component({
  selector: 'change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
  animations   : fuseAnimations
})
export class ChangePasswordComponent implements AfterViewInit, OnDestroy {
  @ViewChild('dialog') template: TemplateRef<any>
  changePasswordForm: FormGroup;

  errors: ValidationErrors[] = []; 
  // Private
   private _unsubscribeAll: Subject<any>;

  constructor(
    private _userDataService: UserDataService,
    private _usersService: UsersService,
    private _dialog: MatDialog,
    private _router: Router,
    private _snackBar: MatSnackBar,
    private _formBuilder: FormBuilder
  ) {

    // Set the private defaults
    this._unsubscribeAll = new Subject();

   }

   /**
     * On init
     */
  ngOnInit(): void
  {
      this.changePasswordForm = this._formBuilder.group({
          oldPassword    : ['', Validators.required],
          password       : ['', [Validators.required, passwordValidator] ],
          passwordConfirm: ['', [Validators.required, confirmPasswordValidator] ]
      });

      // Update the validity of the 'passwordConfirm' field
      // when the 'password' field changes
      this.passwordControl.valueChanges
          .pipe(takeUntil(this._unsubscribeAll))
          .subscribe(() => {
              this.confirmPasswordControl.updateValueAndValidity();
          });
  }
  
  ngAfterViewInit() {
    
    setTimeout(() => {
      const ref = this._dialog.open(this.template, {
        width: '280px'
      });

      ref.afterClosed().subscribe(() => {
        this._router.navigate(['']);
      })
    }, 1000)
  }

  get oldPasswordControl() {
    return this.changePasswordForm.get('oldPassword');
  }

  get passwordControl()  {
    return this.changePasswordForm.get('password');
  }

  get confirmPasswordControl() {
    return this.changePasswordForm.get('passwordConfirm')
  }

  changePassword() {
    
    var data = this.changePasswordForm.getRawValue();

    var formData = {
      username: this._userDataService.getUsername(),
      oldPassword: data.oldPassword,
      password: data.password
    }
    
    this._usersService.changePassword(formData)
      .then((response: any) => {
        
        if (response.payload.length > 0) {

          this._snackBar.open(response.payload, 'Ok', {
            verticalPosition: 'top',
            duration: 2000
          });

          this._dialog.closeAll();

        }
        else if (response.errors.message.length > 0) {

          if (response.errors.message[0].code == 'PasswordMismatch') {
            this.oldPasswordControl.setErrors({ passwordMismatch: {message: response.errors.message[0].description} });
          }else {
            var messages = "";
            _.forEach(response.errors.message, (item, index) => {
             
              this.passwordControl.setErrors({passwordError: { message: 'fix it!'}});
              this.confirmPasswordControl.setErrors({passwordError: { message: 'fix it!'}});
             
              switch (item.code){
                case "PasswordTooShort":
                  
                  // this.errors.push({ passwordTooShort: {message: item.description}});
                  messages += `${item.description}\n`;
                  break;

                case "PasswordRequiresNonAlphanumeric":
                  // this.errors.push({ passwordRequiresNonAlphanumeric: {message: item.description} })
                  messages += `${item.description}\n`
                  break;

                case "PasswordRequiresDigit":
                  // this.errors.push({ passwordRequiresDigit: {message: item.description} });
                  messages += `${item.description}\n`
                  break;

                case "PasswordRequiresUpper":
                  // this.errors.push({ passwordRequiresUpper: {message: item.description} });
                  messages += `${item.description}\n`
                  break;
              }
          });

          this._snackBar.open(messages, 'Ok', {
            verticalPosition: 'top',
            duration: 7000
          });

          }
          
        }

      })
  
  }
      /**
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

}



/**
 * password validator
 *
 * @param {AbstractControl} control
 * @returns {ValidationErrors | null}
 */
export const passwordValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {

  if ( !control.parent || !control )
  {
      return null;
  }

  const oldPassword = control.parent.get('oldPassword');
  const password = control.parent.get('password');

  if ( !password || !oldPassword)
  {
      return null;
  }

  if ( oldPassword.value === '')
  {
    return null;
  }

  if ( password.value !== oldPassword.value )
  {
    return null;
  }

  return {'newPasswordSameAsOldPassword': true};
};

/**
 * Confirm password validator
 *
 * @param {AbstractControl} control
 * @returns {ValidationErrors | null}
 */
export const confirmPasswordValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {

  if ( !control.parent || !control )
  {
      return null;
  }

  const password = control.parent.get('password');
  const passwordConfirm = control.parent.get('passwordConfirm');

  if ( !password || !passwordConfirm)
  {
      return null;
  }

  if ( passwordConfirm.value === '' )
  {
      return null;
  }

  if ( password.value === passwordConfirm.value )
  {
      return null;
  }


  return {'passwordsNotMatching': true};
};