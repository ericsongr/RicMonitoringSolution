import { FuseUtils } from "@fuse/utils";
import { BillingStatement } from "../rent-transactions/billing-statement/billing-statement.model";
import { RentTransactionPayment } from "./rent-transaction.payment.model";

export class RentTransaction {
    id                  : number;
    renterName          : string;
    renterId            : number;
    roomName            : string;
    roomId              : number;
    monthlyRent         : number;
    dueDate             : string;
    dueDateString       : string;
    period              : string;
    paidDate            : string;
    paidDateInput       : string;
    paidAmount          : number;
    totalPaidAmount     : number;
    balanceDateToBePaid : string;
    balanceDateToBePaidInput : string;
    balance             : number;
    totalAmountDue      : number;
    rentArrearId        : number;
    previousUnpaidAmount: number;
    isDepositUsed       : boolean;
    note                : string;
    transactionType     : number;
    isNoAdvanceDepositLeft : boolean;
    isProcessed         : boolean;
    billingStatement    : BillingStatement;
    payments            : RentTransactionPayment[];

    isAddingPayment             : boolean;
    isEditingPayment            : boolean;
    rentTransactionPaymentId    : number;
    handle                      : string;
    

    constructor(transaction?) {
        transaction = transaction || {};
        this.id = transaction.id || 0;
        this.renterName = transaction.renterName || '';
        this.renterId = transaction.renterId;
        this.roomName = transaction.roomName;
        this.roomId = transaction.roomId;
        this.monthlyRent = transaction.monthlyRent;
        this.dueDate = transaction.dueDate;
        this.dueDateString = transaction.dueDateString;
        this.period = transaction.period;
        this.balanceDateToBePaid = transaction.balanceDateToBePaid;
        this.balance = transaction.balance;
        this.totalPaidAmount = transaction.paidAmount;
        this.totalAmountDue = transaction.totalAmountDue;
        this.rentArrearId = transaction.rentArrearId;
        this.previousUnpaidAmount = transaction.previousUnpaidAmount;
        this.note = transaction.note;
        this.transactionType = transaction.transactionType;
        this.isNoAdvanceDepositLeft = transaction.isNoAdvanceDepositLeft;
        this.isProcessed = transaction.isProcessed;
        this.billingStatement = transaction.billingStatement;
        this.payments = transaction.payments;
        this.handle = transaction.handle || FuseUtils.handleize(this.renterName);
        
    }

}
