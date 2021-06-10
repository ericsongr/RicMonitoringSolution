import { Component } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
import { fuseAnimations } from '@fuse/animations';

@Component({
  selector: 'app-dialog-delete-payment-confirmation',
  templateUrl: './dialog-delete-payment-confirmation.component.html',
  styleUrls: ['./dialog-delete-payment-confirmation.component.scss'],
  animations: fuseAnimations,
})
export class DialogDeletePaymentConfirmationComponent {

  constructor(
    private dialogRef: MatDialogRef<DialogDeletePaymentConfirmationComponent>
  ) { }

  confirm() {
    this.dialogRef.close('ConfirmedYes');
  }
}
