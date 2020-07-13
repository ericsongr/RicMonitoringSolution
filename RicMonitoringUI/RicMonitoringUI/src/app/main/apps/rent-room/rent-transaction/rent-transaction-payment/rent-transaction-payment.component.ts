import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { RentTransactionPayment } from '../rent-transaction.payment.model';
import { fuseAnimations } from '@fuse/animations';

@Component({
  selector: 'rent-transaction-payment-table',
  templateUrl: './rent-transaction-payment.component.html',
  styleUrls: ['./rent-transaction-payment.component.scss'],
  animations: fuseAnimations
})
export class RentTransactionPaymentComponent {
  
  @Input() public payments: RentTransactionPayment[];
  @Input() public isHiddenButtons: boolean;
  @Output() public  editPayment: EventEmitter<number> = new EventEmitter<number>() ;
  @Output() public deletePayment: EventEmitter<number> = new EventEmitter<number>();

  displayedColumns: string[] = ['id','datePaid','datePaidString','amount','paymentTransactionType', 'edit_delete_icon'];

  constructor() { }

  onClickEditPayment(paymentId) {
    this.editPayment.emit(paymentId);
  }

  onClickDeletePayment(paymentId) {
    this.deletePayment.emit(paymentId);
  }

  getTotalAmount() {
    return this.payments.map(t => t.amount).reduce((acc, value) => acc + value, 0);
  }

}
