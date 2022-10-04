import { FuseUtils } from "@fuse/utils";

export class User {
    id          : string;
    firstName   : string;
    lastName    : string;
    email       : string;
    mobileNumber: string;
    phoneNumber : string;
    userName    : string;
    role        : string;
    handle      : string;
    isReceiveDueDateAlertPushNotification   : boolean;
    isPaidPushNotification   : boolean;

    constructor(user?) {
        user = user || {};
        this.id = user.id || "new";
        this.firstName = user.firstName;
        this.lastName = user.lastName || "";
        this.email = user.email;
        this.mobileNumber = user.mobileNumber;
        this.phoneNumber = user.phoneNumber;
        this.userName = user.userName;
        this.role = user.role;
        this.isReceiveDueDateAlertPushNotification = user.isReceiveDueDateAlertPushNotification;
        this.isPaidPushNotification = user.isPaidPushNotification;
        this.handle = user.handle || FuseUtils.handleize(this.lastName);
    }

}
