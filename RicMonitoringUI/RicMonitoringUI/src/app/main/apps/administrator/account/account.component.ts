import { Component, OnInit, OnDestroy } from '@angular/core';
import { Account } from './account.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { AccountService } from './account.service';
import { MatSnackBar } from '@angular/material';
import { FuseUtils } from '@fuse/utils';
import { Location } from '@angular/common';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit, OnDestroy {
  
  account = new Account();
  pageType: string;
  accountForm: FormGroup;
  onAccountChanged: Subscription;
  
  constructor(
    private _accountService: AccountService,
    private _formBuilder: FormBuilder,
    private _snackBar : MatSnackBar,
    private _location : Location
    ) { }
    
    ngOnInit() {
      this.onAccountChanged =
        this._accountService.onAccountChanged
            .subscribe(account => {
              if(account) {
                this.account = new Account(account);
                this.pageType = 'edit';
              } 
              else 
              {
                this.pageType = 'add';
                this.account = new Account();
                
              }

              this.accountForm = this.createAccountForm();
            });
    }

  createAccountForm(): FormGroup {
    return this._formBuilder.group({
      id : [this.account.id],
      name : [this.account.name, Validators.required],
      timezone : [this.account.timezone, Validators.required],
      isActive : [this.account.isActive],
      lookup : [this.account.lookup, Validators.required],
      street : [this.account.street, Validators.required],
      subUrb : [this.account.subUrb, Validators.required],
      state : [this.account.state, Validators.required],
      postalCode : [this.account.postalCode, Validators.required],
      email : [this.account.email, [Validators.required, Validators.email]],
      phoneNumber : [this.account.phoneNumber],
      websiteUrl : [this.account.websiteUrl],
      facebookUrl : [this.account.facebookUrl],
      addressLine1 : [this.account.addressLine1],
      city : [this.account.city],
      dialingCode : [this.account.dialingCode],
      businessOwnerName : [this.account.businessOwnerName],
      businessOwnerPhoneNumber : [this.account.businessOwnerPhoneNumber],
      businessOwnerEmail : [this.account.businessOwnerEmail],
      geoCoordinates : [this.account.geoCoordinates],
      companyFeeFailedPaymentCount : [this.account.companyFeeFailedPaymentCount],
      paymentIssueSuspensionDate : [this.account.paymentIssueSuspensionDate],
    });
  }
  
  save() {
    if (this.accountForm.invalid) {
      
      //show the success message
      this._snackBar.open('Invalid data. Please verify.', 'OK', {
        verticalPosition  : 'top',
        duration          : 2000,
        panelClass        : ['mat-warn']
      });

    } else {

      const data = this.accountForm.getRawValue();
      data.handle = FuseUtils.handleize(data.name);

      this._accountService.saveAccount(data)
          .then((response: any) => {
            if (!response.errors.message) {
                //Trigger the subscription with new data
                this._accountService.onAccountChanged.next(data);

                //show the success message
                this._snackBar.open('Account detail saved.', 'OK', {
                  verticalPosition  : 'top',
                  duration          : 2000
                });

                //change the location with new one
                this._location.go(`/administrator/account/${this.account.id}/${this.account.handle}`);

              } else {
                  this._snackBar.open('error occurred, updating account detail.', 'OK', {
                    verticalPosition  : 'top',
                    duration          : 2000
                  });
              }
          });
      }
  }

  add() {

    if (this.accountForm.invalid) {
      
      //show the success message
      this._snackBar.open('Invalid data. Please verify.', 'OK', {
        verticalPosition  : 'top',
        duration          : 2000,
        panelClass        : ['mat-warn']
      });

    } else {

      const data = this.accountForm.getRawValue();
      data.handle = FuseUtils.handleize(data.name);
  
      this._accountService.addAccount(data)
          .then((response: any) => {
            if (!response.errors.message) {
                //Trigger the subscription with new data
                this._accountService.onAccountChanged.next(data);

                var accountId = parseInt(response.payload);
                //show the success message
                this._snackBar.open('New account added.', 'OK', {
                  verticalPosition  : 'top',
                  duration          : 2000
                });

                //change the location with new one
                this._location.go(`/apartment/accounts/${accountId}/${this.account.handle}`);
            }
            else 
            {
                this._snackBar.open('error occurred, adding account detail.', 'OK', {
                  verticalPosition  : 'top',
                  duration          : 2000
                });
            }
            
          });
    }
    
  }

  

  ngOnDestroy(): void {
    this.onAccountChanged.unsubscribe();
  }
}
