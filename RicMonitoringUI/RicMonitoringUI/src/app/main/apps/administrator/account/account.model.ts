import { FuseUtils } from "@fuse/utils";

export class Account {
    id : number
    name : string
    timezone : string
    isActive : boolean
    isSelected : boolean
    lookup : string
    street : string
    subUrb : string
    state : string
    postalCode : string
    email : string
    phoneNumber : string
    websiteUrl : string
    facebookUrl : string
    addressLine1 : string
    city : string
    dialingCode : string
    businessOwnerName : string
    businessOwnerPhoneNumber : string
    businessOwnerEmail : string
    geoCoordinates : string
    companyFeeFailedPaymentCount : number
    paymentIssueSuspensionDate : Date
    handle : string

    constructor(account?) {
        account = account || {};
        this.id = account.id || 0
        this.name = account.name || ''
        this.timezone = account.timezone
        this.isActive = account.isActive
        this.isSelected = account.isSelected
        this.street = account.street
        this.subUrb = account.subUrb
        this.state = account.state
        this.postalCode = account.postalCode
        this.email = account.email
        this.phoneNumber = account.phoneNumber
        this.websiteUrl = account.websiteUrl
        this.facebookUrl = account.facebookUrl
        this.addressLine1 = account.addressLine1
        this.city = account.city
        this.dialingCode = account.dialingCode
        this.businessOwnerName = account.businessOwnerName
        this.businessOwnerPhoneNumber = account.businessOwnerPhoneNumber
        this.businessOwnerEmail = account.businessOwnerEmail
        this.geoCoordinates = account.geoCoordinates
        this.companyFeeFailedPaymentCount = account.companyFeeFailedPaymentCount
        this.paymentIssueSuspensionDate = account.paymentIssueSuspensionDate

        this.handle = account.handle || FuseUtils.handleize(this.name);
    }

}
