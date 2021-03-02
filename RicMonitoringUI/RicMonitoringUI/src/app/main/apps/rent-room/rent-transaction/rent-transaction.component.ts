import { Component, OnInit, OnDestroy, AfterViewInit, ChangeDetectorRef, Input } from '@angular/core';
import { RentTransaction } from './rent-transaction.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { RentTransactionService } from './rent-transaction.service';
import { MatSnackBar, MatRadioChange, MatDialog } from '@angular/material';
import { TransactionTypeEnum } from '../../common/enums/transaction-type.enum';
import { BillingStatement } from '../rent-transactions/billing-statement/billing-statement.model';
import { ActivatedRoute, Router } from '@angular/router';
import { RentTransactionPayment } from './rent-transaction.payment.model';
import { DialogDeletePaymentConfirmationComponent } from './dialog-delete-payment-confirmation/dialog-delete-payment-confirmation.component';
import { fuseAnimations } from '@fuse/animations';
import * as moment from 'moment';

@Component({
  selector: 'page-rent-transaction',
  templateUrl: './rent-transaction.component.html', 
  styleUrls: ['./rent-transaction.component.scss'],
  animations: fuseAnimations
})
export class RentTransactionComponent implements OnInit, OnDestroy, AfterViewInit {
  
  profileImage: string = 'assets/images/avatars/profile.jpg';
  rentTransaction = new RentTransaction();
  pageType: string;
  rentTransactionForm: FormGroup;
  onRentTransactionChanged: Subscription;
  rooms: any;
  
  isShowBreakdown: boolean = false;
  monthlyRentPrice: number;
  hasBalance: boolean;
  isTransactionHasBeenCompleted: boolean = false;
  isAddingPayment: boolean = false;
  isEditingPayment: boolean = false;

  currentTotalPaidAmount: number;
  paidDate: Date;
  datePaidLabel: string = 'Date Paid';

  billingStatement: BillingStatement;

  //payments table
  payments: RentTransactionPayment[];

  constructor(
    private _rentTransactionService: RentTransactionService,
    private _dialog: MatDialog,
    private _route: ActivatedRoute,
    private _router: Router,
    private _formBuilder: FormBuilder,
    private _snackBar : MatSnackBar,
    private _cdr: ChangeDetectorRef
    ) { }
    
    ngOnInit() {
     
      this.onRentTransactionChanged =
        this._rentTransactionService.onRentTransactionChanged
            .subscribe(transaction => {
              
              this.rentTransaction = new RentTransaction(transaction);
              this.payments = this.rentTransaction.payments;
              this.profileImage = this.rentTransaction.base64;
              
              if (this.rentTransaction.paidDate != "")
              {
                this.paidDate = new Date();
              }
              
              this.rentTransactionForm = this.createRentTransactionForm();
              
              if (this.rentTransaction.id > 0) {
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
    this.isAddingPayment = true;
  }

  cancelPayment() {
    this.isTransactionHasBeenCompleted = true;
    this.isAddingPayment = false;
    this.isEditingPayment = false;

    this.rentTransaction.rentTransactionPaymentId = 0;
    this.rentTransaction.paidAmount = null;
    this.rentTransaction.paidDate = null;

    if(this.rentTransaction.isDepositUsed) {

      this.rentTransaction.isDepositUsed = false;

      this.onChangeDepositUsed(false);
    }

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

      this.rentTransaction.paidDateInput = moment(formData.paidDate).format('YYYY-MM-DD');
      this.rentTransaction.balanceDateToBePaidInput = moment(formData.balanceDateToBePaid).format('YYYY-MM-DD');
      
      this.rentTransaction.paidAmount = formData.paidAmount;
      this.rentTransaction.note = formData.note;
      this.rentTransaction.transactionType = TransactionTypeEnum.MonthlyRent;
      this.rentTransaction.isAddingPayment = this.isAddingPayment;
      this.rentTransaction.isEditingPayment = this.isEditingPayment;

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
              this._snackBar.open(message,
                  'OK',
                  {
                      verticalPosition: 'top',
                      duration: 2000
                  });

          });

      setTimeout(() => {
              this._router.navigate([`apartment/tenant-transactions`]);
          },
          1000);
    }
}

editPayment(paymentId) {
  var payment = this.payments.find(o => o.id == paymentId);
  if (payment != null) {
    
    this.rentTransaction.rentTransactionPaymentId = payment.id;
    this.rentTransaction.paidAmount = payment.amount;
    this.rentTransaction.paidDate = payment.datePaid;

    this.isTransactionHasBeenCompleted = false;
    this.isEditingPayment = true;

  }

}

deletePayment(paymentId) {
  var confirmationDialog = this._dialog.open(DialogDeletePaymentConfirmationComponent, {
    width: '350px'
  });

  confirmationDialog.afterClosed().subscribe(result => {
    if (result == "ConfirmedYes") {

      this._rentTransactionService.deletePayment(paymentId, this.rentTransaction.renterId)
        .then((response: any) => {

          if (!response.errors.message) {
            this._snackBar.open("Payment has been deleted.", 'OK', {
              verticalPosition  : 'top',
              duration          : 2000
            });
            
            this._router.navigate([`apartment/tenant-transactions`]);
          }
        });

        
    }
  })

  
}

  get paidAmount() {
    return this.rentTransactionForm.get('paidAmount');
  }

  get paidDateDatePicker() {
    return this.rentTransactionForm.get('paidDate');
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

      this.paidDateDatePicker.setValidators(null);
      this.paidAmount.setValidators(null);
      
    
    } else {
      this.paidDateDatePicker.setValidators([Validators.required]);
      this.paidAmount.setValidators([Validators.required]);
    }

    this.paidDateDatePicker.updateValueAndValidity();
    this.paidAmount.updateValueAndValidity();
    
  }

  ngOnDestroy(): void {
    this.onRentTransactionChanged.unsubscribe();
  }
}
