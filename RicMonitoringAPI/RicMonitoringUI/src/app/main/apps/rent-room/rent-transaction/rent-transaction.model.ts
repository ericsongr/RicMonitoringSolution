import { FuseUtils } from "@fuse/utils";
import { BillingStatement } from "../rent-transactions/billing-statement/billing-statement.model";

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
    paidAmount          : number;
    balanceDateToBePaid : string;
    balance             : number;
    isBalanceEditable   : boolean;
    totalAmountDue      : number;
    previousUnpaidAmount: number;
    isDepositUsed       : boolean;
    note                : string;
    adjustmentBalancePaymentDueAmount   : number;
    transactionType     : number;
    isNoAdvanceDepositLeft : boolean;
    isProcessed         : boolean;
    billingStatement    : BillingStatement;
    handle              : string;
    

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
        this.paidDate = transaction.paidDate;
        this.balanceDateToBePaid = transaction.balanceDateToBePaid;
        this.balance = transaction.balance;
        this.isBalanceEditable = transaction.isBalanceEditable;
        this.totalAmountDue = transaction.totalAmountDue;
        this.previousUnpaidAmount = transaction.previousUnpaidAmount;
        this.paidAmount = transaction.paidAmount || 
            (transaction.isDepositUsed ? 0 : 
                (this.monthlyRent == this.totalAmountDue ? transaction.monthlyRent : 0));
        this.isDepositUsed = transaction.isDepositUsed || false;
        this.note = transaction.note;
        this.adjustmentBalancePaymentDueAmount = transaction.adjustmentBalancePaymentDueAmount;
        this.transactionType = transaction.transactionType;
        this.isNoAdvanceDepositLeft = transaction.isNoAdvanceDepositLeft;
        this.isProcessed = transaction.isProcessed;
        this.billingStatement = transaction.billingStatement;
        this.handle = transaction.handle || FuseUtils.handleize(this.renterName);
        
    }

}
