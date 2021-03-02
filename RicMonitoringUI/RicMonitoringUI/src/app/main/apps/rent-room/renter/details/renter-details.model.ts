import { FuseUtils } from "@fuse/utils";

export class RenterDetail {
    id              : number;
    name            : string;
    advanceMonths   : number;
    monthsUsed      : number;
    startDate       : string;
    startDateString : string;
    advancePaidDate : string;
    advancePaidDateString : string;
    dueDay         : number;
    noOfPersons     : number;
    roomId          : number;
    isEndRent       : boolean;
    dateEndRent     : string;
    totalAdvanceAmountDue : number;
    totalPaidAmount       : number;
    balanceAmount         : number;
    balancePaidDate       : string;
    base64                : string;
    handle                : string;
    
    constructor(renter?) {
        renter = renter || {};
        this.id = renter.id || 0;
        this.name = renter.name || '';
        this.advanceMonths = renter.advanceMonths;
        this.monthsUsed = renter.monthsUsed;
        this.advancePaidDate = renter.advancePaidDate;
        this.startDate = renter.startDate;
        this.dueDay = renter.dueDay;
        this.startDate = renter.startDate;
        this.noOfPersons = renter.noOfPersons;
        this.roomId = renter.roomId;
        this.isEndRent = renter.isEndRent || false;
        this.dateEndRent = renter.dateEndRent;
        this.totalAdvanceAmountDue = renter.totalAdvanceAmountDue;
        this.totalPaidAmount = renter.totalPaidAmount || 0;
        this.balanceAmount = renter.balanceAmount;
        this.balancePaidDate = renter.balancePaidDate;
        this.base64 = renter.base64;
        this.handle = renter.handle || FuseUtils.handleize(this.name);
    }

}
