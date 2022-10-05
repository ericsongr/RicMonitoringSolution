import { FuseUtils } from "@fuse/utils";
import { UserRegistedDevice } from "./user-registered-device.model";

export class UserProfile {
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
    isPaidPushNotification                  : boolean;
    isIncomingDueDatePushNotification                  : boolean;
    registeredDevices: UserRegistedDevice[]

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
        this.isReceiveDueDateAlertPushNotification = user.isReceiveDueDateAlertPushNotification || false;
        this.isPaidPushNotification = user.isPaidPushNotification || false;
        this.isIncomingDueDatePushNotification = user.isIncomingDueDatePushNotification || false;
        this.registeredDevices = user.registeredDevices;
        this.handle = user.handle || FuseUtils.handleize(this.lastName);
    }

}
