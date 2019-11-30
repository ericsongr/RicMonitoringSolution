import { FuseUtils } from "@fuse/utils";

export class Renter {
    id              : number;
    name            : string;
    advanceMonths   : number;
    monthsUsed      : number;
    startDate       : string;
    advancePaidDate : string;
    dueDate         : string;
    noOfPersons     : number;
    roomId          : number;
    isEndRent       : boolean;
    dateEndRent     : string;
    totalAdvanceAmountDue : number;
    totalPaidAmount       : number;
    balanceAmount         : number;
    balancePaidDate       : string;
    handle                : string;
    
    constructor(renter?) {
        renter = renter || {};
        this.id = renter.id || 0;
        this.name = renter.name || '';
        this.advanceMonths = renter.advanceMonths;
        this.monthsUsed = renter.monthsUsed;
        this.advancePaidDate = renter.advancePaidDate;
        this.startDate = renter.startDate;
        this.dueDate = renter.dueDate;
        this.startDate = renter.startDate;
        this.noOfPersons = renter.noOfPersons;
        this.roomId = renter.roomId;
        this.isEndRent = renter.isEndRent || false;
        this.dateEndRent = renter.dateEndRent;
        this.totalAdvanceAmountDue = renter.totalAdvanceAmountDue;
        this.totalPaidAmount = renter.totalPaidAmount || 0;
        this.balanceAmount = renter.balanceAmount;
        this.balancePaidDate = renter.balancePaidDate;
        this.handle = renter.handle || FuseUtils.handleize(this.name);
    }

}
