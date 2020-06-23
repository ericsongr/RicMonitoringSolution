import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-dialog-delete-payment-confirmation',
  templateUrl: './dialog-delete-payment-confirmation.component.html',
  styleUrls: ['./dialog-delete-payment-confirmation.component.scss']
})
export class DialogDeletePaymentConfirmationComponent {

  constructor(
    private dialogRef: MatDialogRef<DialogDeletePaymentConfirmationComponent>
  ) { }

  confirm() {
    this.dialogRef.close('ConfirmedYes');
  }
}
