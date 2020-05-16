import { Component, OnInit, ChangeDetectorRef, Input } from '@angular/core';
import { BillingStatement } from './billing-statement.model';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'button-billing-statement',
  templateUrl: './billing-statement.component.html',
  styleUrls: ['./billing-statement.component.scss']
})
export class BillingStatementComponent implements OnInit {
  @Input()
    public billingStatement: BillingStatement;
    public status: string;

  text: string;

  constructor(
    private _snackBar: MatSnackBar,
    private _cdr: ChangeDetectorRef,
  ) {
   
  }

  ngOnInit() {
    this.billingStatementToClipboard(this.billingStatement);
  }

  billingStatementToClipboard(viewModel: BillingStatement) {

    this.text = `${viewModel.renterName}\n` +
      `=======================\n` +
      `Room : ${viewModel.roomName}\n` +
      `Period: ${viewModel.period}\n` +
      `Due Date: ${viewModel.dueDate}\n` +
      `Rent: ${viewModel.monthlyRent}\n`;

      if (viewModel.hasPreviousBalance) {
        this.text += 
          `Balance: ${viewModel.previousUnpaidAmount}\n` +
          `=======================\n` +
          `Total   ${viewModel.totalAmountDue}\n`;
      }

  }
  
  public notify(event: string): void {

    let message = `'${event}' has been copied to clipboard`
    
    this.status = message;

    this._snackBar.open("Billing statement has been copied to clipboard", 'OK', {
      verticalPosition: 'top',
      duration: 2000
    });
    this._cdr.detectChanges();

  }

}
