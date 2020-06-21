import { Component, OnInit, OnDestroy, AfterViewInit, ChangeDetectorRef, Input } from '@angular/core';
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
import { BillingStatement } from '../rent-transactions/billing-statement/billing-statement.model';
import { ActivatedRoute } from '@angular/router';
import { RentTransactionPayment } from './rent-transaction.payment.model';

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
  isTransactionHasBeenCompleted: boolean = false;
  isAddingAdvancePayment: boolean = false;

  currentTotalPaidAmount: number;
  paidDate: Date;
  datePaidLabel: string = 'Date Paid';
  monthFilter: string;

  billingStatement: BillingStatement;

  //payments table
  payments: RentTransactionPayment[];
  displayedColumns: string[] = ['id','datePaid','amount','paymentTransactionType'];

  constructor(
    private _rentTransactionService: RentTransactionService,
    private _route: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _snackBar : MatSnackBar,
    private _location : Location,
    private _cdr: ChangeDetectorRef
    ) { }
    
    ngOnInit() {
      
      this._route.params.subscribe((params) => {
        this.monthFilter = params['monthFilter'];
      })

      this.onRentTransactionChanged =
        this._rentTransactionService.onRentTransactionChanged
            .subscribe(transaction => {
              
              this.rentTransaction = new RentTransaction(transaction);
              this.payments = this.rentTransaction.payments;
              
              if (this.rentTransaction.paidDate != "")
              {
                this.paidDate = new Date();
              }
              
              this.rentTransactionForm = this.createRentTransactionForm();
              
              if (this.rentTransaction.isDepositUsed) {
                this.isTransactionHasBeenCompleted = true;
              }else{
                this.onChangePaidAmount();
              }
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

  addAdvancePayment() {
    this.isTransactionHasBeenCompleted = false;
    this.isAddingAdvancePayment = true;
  }

  cancelAdvancePayment() {
    this.isTransactionHasBeenCompleted = true;
    this.isAddingAdvancePayment = false;
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
      this.rentTransaction.note = formData.note;
      this.rentTransaction.transactionType = TransactionTypeEnum.MonthlyRent;
      this.rentTransaction.isAddingAdvancePayment = this.isAddingAdvancePayment;

      if (!this.hasBalance) {
        this.rentTransaction.balanceDateToBePaid = null;
        this.rentTransaction.balance = null;
      }

      this._rentTransactionService.saveTransaction(this.rentTransaction)
          .then((transactionId: number) => {
            
            this.rentTransaction.id = transactionId;

            this._rentTransactionService.onRentTransactionChanged.next(this.rentTransaction);
            
            var message = "";
            if (this.rentTransaction.id > 0) {
              message = "Transaction detail has been updated."
            } else {
              message = "Transaction has been added."
            }

            //show the success message
            this._snackBar.open(message, 'OK', {
              verticalPosition  : 'top',
              duration          : 2000
            });

          })
          
          this._location.go(`/apartment/tenant-transactions/${this.rentTransaction.renterId}/${this.rentTransaction.handle}`);
          
          this._cdr.detectChanges();
    }
}

  get paidAmount() {
    return this.rentTransactionForm.get('paidAmount');
  }

  get balanceDateToBePaid() {
    return this.rentTransactionForm.get('balanceDateToBePaid');
  }

  onChangePaidAmount() {
    
    if (this.rentTransaction.isProcessed) {
      //isProcessed meaning the batch file has been run.
      this.hasBalance = this.rentTransaction.balance > 0;
    } else {
      //not yet run the batch file
      if (this.rentTransaction.isDepositUsed) {
        this.hasBalance = false;
      } else {
        this.currentTotalPaidAmount = Number(this.rentTransaction.paidAmount) + Number(this.rentTransaction.totalPaidAmount)
        this.hasBalance = Number(this.rentTransaction.totalAmountDue) > this.currentTotalPaidAmount;
      }
      
      if (this.rentTransaction.transactionType == TransactionTypeEnum.AdvanceAndDeposit){
        this.hasBalance = this.rentTransaction.balance > 0;
      }
  
      if (!this.hasBalance) {
        this.balanceDateToBePaid.setValidators(null);
        this.rentTransaction.balanceDateToBePaid = null;
      }
      else {
  
        if (this.rentTransaction.transactionType == TransactionTypeEnum.MonthlyRent) {
          this.currentTotalPaidAmount = Number(this.rentTransaction.paidAmount) + Number(this.rentTransaction.totalPaidAmount);
          this.rentTransaction.balance = 
            (this.currentTotalPaidAmount >  Number(this.rentTransaction.totalAmountDue)) ? 0 : (Number(this.rentTransaction.totalAmountDue) - this.currentTotalPaidAmount)
        }
        
          this.balanceDateToBePaid.setValidators([Validators.required])
      }
      this.balanceDateToBePaid.updateValueAndValidity();

    }
    
    this._cdr.detectChanges();

  }

  onChangeDepositUsedEvent(event: MatRadioChange) {
    
    this.rentTransaction.isDepositUsed = event.value == "true";

    this.onChangeDepositUsed(this.rentTransaction.isDepositUsed)

    this.onChangePaidAmount()
  }
 
  onChangeDepositUsed(isUsedDeposit: boolean) {
    if (isUsedDeposit) {
      this.rentTransaction.isNoAdvanceDepositLeft = false;
      this.rentTransaction.paidAmount = 0;
      this.hasBalance = false;
      this.rentTransaction.balance = 0;
      this.balanceDateToBePaid.setValidators(null);
      this.balanceDateToBePaid.updateValueAndValidity();

      this.rentTransaction.balanceDateToBePaid = null;
      this.paidAmount.disable();
    } else {
      this.rentTransaction.paidAmount = 
        (this.rentTransaction.monthlyRent == this.rentTransaction.totalAmountDue ? this.rentTransaction.monthlyRent : 0);
      this.paidAmount.enable();
    }
    
  }

  getTotalAmount() {
    return this.payments.map(t => t.amount).reduce((acc, value) => acc + value, 0);
  }

  ngOnDestroy(): void {
    this.onRentTransactionChanged.unsubscribe();
  }
}
