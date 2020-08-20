import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-dialog-payments',
  templateUrl: './dialog-payments.component.html',
  styleUrls: ['./dialog-payments.component.scss']
})
export class DialogPaymentsComponent {
  displayedColumns: string[] = ['datePaidString', 'amount', 'paymentTransactionType']
  dataSource: any;
  payments: any;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { 
    this.dataSource = data;
  }

  getTotalPaidAmount() {
    var totalPaidAmount = this.dataSource.map(o => o.amount).reduce((total, value) => total + value, 0);
    return totalPaidAmount;
  }    

}
