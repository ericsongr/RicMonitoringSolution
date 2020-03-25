import { Component, OnInit, OnDestroy, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { RentTransaction } from './rent-transaction.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { RentTransactionService } from './rent-transaction.service';
import { MatSnackBar, MatRadioChange } from '@angular/material';
import { FuseUtils } from '@fuse/utils';
import { Location } from '@angular/common';
import { RoomsService } from '../rooms/rooms.service';
import * as moment from 'moment';
import { TransactionTypeEnum } from '../../common/enums/transaction-type.enum';

@Component({
  selector: 'page-rent-transaction',
  templateUrl: './rent-transaction.component.html', 
  styleUrls: ['./rent-transaction.component.scss']
})
export class RentTransactionComponent implements OnInit, OnDestroy, AfterViewInit {
  
  rentTransaction = new RentTransaction();
  pageType: string;
  rentTransactionForm: FormGroup;
  onRentTransactionChanged: Subscription;
  rooms: any;
  
  isShowBreakdown: boolean = false;
  monthlyRentPrice: number;
  hasBalance: boolean;
  
  defaultPaidDate: Date;

  constructor(
    private _rentTransactionService: RentTransactionService,
    private _roomsService: RoomsService,
    private _formBuilder: FormBuilder,
    private _snackBar : MatSnackBar,
    private _location : Location,
    private _cdr: ChangeDetectorRef
    ) { }
    
    ngOnInit() {
      
      this.onRentTransactionChanged =
        this._rentTransactionService.onRentTransactionChanged
            .subscribe(transaction => {
              this.rentTransaction = new RentTransaction(transaction);
              console.log(transaction);
              if (this.rentTransaction.paidDate != "")
              {
                this.defaultPaidDate = new Date();
              }
              
              this.rentTransactionForm = this.createRentTransactionForm();

              this.onChangePaidAmount();

            });
        
         
  
    }

    ngAfterViewInit(): void {
      
    }
    
    createRentTransactionForm(): FormGroup {

        return this._formBuilder.group({
            paidDate             : [this.rentTransaction.paidDate, Validators.required],
            paidAmount           : [this.rentTransaction.paidAmount, Validators.required],
            balanceDateToBePaid  : [this.rentTransaction.balanceDateToBePaid],
            isDepositUsed        : [this.rentTransaction.isDepositUsed],
            note                 : [this.rentTransaction.note]
        });

  }
  
  save() {

    if (this.rentTransactionForm.invalid){
        this._snackBar.open('Invalid form. Please verify.', 'OK', {
          verticalPosition  : 'top',
          duration          : 2000
        });

        this._cdr.detectChanges();
    
      } else {

      const formData = this.rentTransactionForm.getRawValue();

      this.rentTransaction.paidDate = formData.paidDate;
      this.rentTransaction.paidAmount = formData.paidAmount;
      this.rentTransaction.balanceDateToBePaid = formData.balanceDateToBePaid;
      this.rentTransaction.isDepositUsed = formData.isDepositUsed;
      this.rentTransaction.note = formData.note;
      this.rentTransaction.transactionType = TransactionTypeEnum.MonthlyRent;

      if (!this.hasBalance) {
        this.rentTransaction.balanceDateToBePaid = null;
        this.rentTransaction.balance = null;
      }

      this._rentTransactionService.saveTransaction(this.rentTransaction)
          .then((transaction: RentTransaction) => {
            //Trigger the subscription with new data
            this._rentTransactionService.onRentTransactionChanged.next(transaction);
            var message = "";
            if (this.rentTransaction.id > 0){
              message = "Transaction detail has been updated."
            } else {
              message = "Transaction has been added."
            }

            this.rentTransaction.id = transaction.id;

            //show the success message
            this._snackBar.open(message, 'OK', {
              verticalPosition  : 'top',
              duration          : 2000
            });

          })
    }
}
  get balanceDateToBePaid() {
    return this.rentTransactionForm.get('balanceDateToBePaid');
  }

  onChangePaidAmount() {
    
    this.hasBalance = Number(this.rentTransaction.monthlyRent) > Number(this.rentTransaction.paidAmount);

    if (this.rentTransaction.transactionType == TransactionTypeEnum.AdvanceAndDeposit){
      this.hasBalance = this.rentTransaction.balance > 0;
    }

    if (!this.hasBalance) {
      this.balanceDateToBePaid.setValidators(null);
      this.rentTransaction.balanceDateToBePaid = null;
    }
    else {

      if (this.rentTransaction.transactionType == TransactionTypeEnum.MonthlyRent){
        this.rentTransaction.balance = 
          (Number(this.rentTransaction.paidAmount) >  Number(this.rentTransaction.monthlyRent)) ? 0 : (Number(this.rentTransaction.monthlyRent) - Number(this.rentTransaction.paidAmount))
      }
      
        this.balanceDateToBePaid.setValidators([Validators.required])
    }
    this.balanceDateToBePaid.updateValueAndValidity();
    this._cdr.detectChanges();

    console.log("this.rentTransaction", this.rentTransaction);
  }

  onChangeDepositUsed(event: MatRadioChange) {
    if (event.value == "true") {
      
      this.rentTransaction.paidAmount = 0;
      this.hasBalance = false;
      this.rentTransaction.balance = 0;
      this.balanceDateToBePaid.setValidators(null);
      this.balanceDateToBePaid.updateValueAndValidity();

      this.rentTransaction.balanceDateToBePaid = null;

    } else {
      this.rentTransaction.paidAmount = this.rentTransaction.monthlyRent;
    }
  }

  ngOnDestroy(): void {
    this.onRentTransactionChanged.unsubscribe();
  }
}
